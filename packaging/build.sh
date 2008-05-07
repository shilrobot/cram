#!/bin/bash

# We have to add this to the $PATH so we can run the VC# Express compiler on the .sln
export PATH="$PATH:/cygdrive/c/program files/microsoft visual studio 8/common7/ide"
VCS=VCSExpress.exe

# Do a subversion export (no .svn directories or intermediate files, 100% clean)
SRC=src
rm -rf Cram-* $SRC md5sum.txt verify 
svn export http://shilbert.cjb.net/repos/private/trunk/csharp/Cram $SRC
if [ $? -ne 0 ] ; then
	echo "*** SVN EXPORT FAILED ***"
	exit 1
fi

# Extract version string from the version info source file
VERSION=`sed -n 's:^.*Version\s*=\s*\"\(.*\)\".*$:\1:p' $SRC/CramGUI/VersionInfo.cs`
if [ "$VERSION" == "" ] ; then
	echo "*** COULD NOT EXTRACT VERSION STRING ***"
	exit 1
fi
echo Version = "$VERSION"

# Create the Cram-X.Y directory.
# We copy the src directory over before we build in it, so the zip file
# gets a clean copy.
# Also, we remove the packaging scripts from the distribution, because
# noone but me should get them (and they also contain info like how to FTP
# into shilbert.com, although the password isn't embedded.)
CRAM_DIST_NAME=Cram-$VERSION
CRAM_ZIP_NAME=$CRAM_DIST_NAME.zip
mkdir $CRAM_DIST_NAME
cp -r $SRC $CRAM_DIST_NAME
rm -rf $CRAM_DIST_NAME/$SRC/packaging

# Build the crammer
pushd $SRC
$VCS Cram.sln /build Release
if [ $? -ne 0 ] ; then
	echo "*** BUILD FAILED ****"
	popd
	exit 1
fi
popd

# Copy EXE files into the root of the distro directory
mkdir $CRAM_DIST_NAME/$BIN
cp $SRC/CramGUI/bin/Release/CramGUI.exe $CRAM_DIST_NAME
cp $SRC/CramCLI/bin/Release/CramCLI.exe $CRAM_DIST_NAME
rm -rf $SRC

# Add everything to a zip file
pushd $CRAM_DIST_NAME
zip -9r ../$CRAM_ZIP_NAME .
popd
md5sum $CRAM_ZIP_NAME > md5sum.txt

# Clean up
rm -rf $CRAM_DIST_NAME


# Upload the final result to shilbert.com
echo "*** UPLOADING TO SHILBERT.COM ***"
./upload.sh $CRAM_ZIP_NAME


mkdir verify
pushd verify
wget -O $CRAM_ZIP_NAME "http://www.shilbert.com/files/$CRAM_ZIP_NAME"
md5sum -c ../md5sum.txt
popd

rm -rf md5sum.txt verify