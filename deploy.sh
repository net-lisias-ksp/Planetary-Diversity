#!/usr/bin/env bash

source ./CONFIG.inc

deploy() {
	local DLL=$1

	if [ -f "./$PROJECTSDIR/bin/Release/$DLL.dll" ] ; then
		cp "./$PROJECTSDIR/bin/Release/$DLL.dll" "./GameData/$TARGETDIR/Plugins"
		if [ -f "${KSP_DEV}/GameData/$TARGETDIR/" ] ; then
			cp "./$PROJECTSDIR/bin/Release/$DLL.dll" "${KSP_DEV/}GameData/$TARGETDIR/Plugins"
		fi
	fi
	if [ -f "./$PROJECTSDIR/bin/Debug/$DLL.dll" ] ; then
		if [ -d "${KSP_DEV}/GameData/$TARGETDIR/" ] ; then
			cp "./$PROJECTSDIR/bin/Debug/$DLL.dll" "${KSP_DEV}GameData/$TARGETDIR/Plugins"
		fi
	fi
}

VERSIONFILE=$PACKAGE.version

cp $VERSIONFILE "./GameData/net.lisias.ksp"
cp README.md "./GameData/$TARGETDIR"
cp CHANGE_LOG.md "./GameData/$TARGETDIR"
cp LICENSE "./GameData/$TARGETDIR"
deploy $PACKAGE
