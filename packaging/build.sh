#!/bin/bash

SRC=src
rm -rf Cram-* $SRC
mkdir $SRC
svn export http://shilbert@shilbert.cjb.net/repos/private/trunk/csharp/Platformer/Cram/ $SRC/Cram
svn export http://shilbert@shilbert.cjb.net/repos/private/trunk/csharp/Platformer/CramCLI/ $SRC/CramCLI
# Extract version string from the version info source file
VERSION=`sed -n 's:^.*Version\s*=\s*\"\(.*\)\".*$:\1:p' $SRC/Cram/VersionInfo.cs`
echo Version = "$VERSION"
CRAM_DIST_NAME=Cram-$VERSION
mkdir $CRAM_DIST_NAME
mv $SRC $CRAM_DIST_NAME

pushd $CRAM_DIST_NAME
zip -9r ../$CRAM_DIST_NAME.zip .
popd
