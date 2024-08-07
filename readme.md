### building:

1. build controller
2. create a folder named '.source' in 'CLIENT'
3. create a file named 'archive.zip' (it's extension must be 'zip') in '.source'
4. add compiled 'CONTROLLER.dll' to '.source'
5. build 'CLIENT'
6. create an archive of 'CLIENT' (must be zip archive)
7. create a folder named '.source' in 'SERVER'
8. move the archive ('CLIENT.zip') to there ('/server/.source')
9. build 'SERVER'

-----

### using:

1. run server.exe
2. press 'E' for an encryption project
3. set source path, dest path, KEY and IV (paths must not have ' " ')