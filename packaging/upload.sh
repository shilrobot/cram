#!/bin/bash

FTPCMDS=.ftpcommands

stty -echo
read -p "Password: " passw; echo
stty echo
 
echo "shilbert
$passw
CD www/files
PUT $1
quit" > "$FTPCMDS"
ftp -s:"$FTPCMDS" shilbert.com
rm -rf $FTPCMDS
