#!/usr/bin/env bash

source ./CONFIG.inc

clean() {
	rm "./GameData/$TARGETDIR/Plugins"/*.dll
	rm "${KSP_DEV}/GameData/$TARGETDIR/Plugins"/*.dll
}

deploy() {
	local DLL=$1

	if [ -f "./bin/Release/$DLL.dll" ] ; then
		cp "./bin/Release/$DLL.dll" "./GameData/$TARGETDIR/Plugins"
		if [ -f "${KSP_DEV}/GameData/$TARGETDIR/" ] ; then
			cp "./bin/Release/$DLL.dll" "${KSP_DEV/}GameData/$TARGETDIR/Plugins"
		fi
	fi
	if [ -f "./bin/Debug/$DLL.dll" ] ; then
		if [ -d "${KSP_DEV}/GameData/$TARGETDIR/" ] ; then
			cp "./bin/Debug/$DLL.dll" "${KSP_DEV}GameData/$TARGETDIR/Plugins"
		fi
	fi
}

VERSIONFILE=$PACKAGE.version

cp $VERSIONFILE "./GameData/net.lisias.ksp"
cp README.md "./GameData/$TARGETDIR"
cp CHANGE_LOG.md "./GameData/$TARGETDIR"
cp LICENSE "./GameData/$TARGETDIR"

clean
for p in $PACKAGE $PACKAGE.API $PACKAGE.CelestialBodies.Atmosphere $PACKAGE.CelestialBodies.GasPlanetColor $PACKAGE.CelestialBodies.Orbit $PACKAGE.Components $PACKAGE.PQSMods ; do
	deploy $p
done
