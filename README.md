# ASU_CSE593_Thesis
This is the code for the CSE 593 Thesis Project

## To Deploy this project:
- download docker desktop for windows or docker for linux
- if you use windows in the C:\Users\<user> directory edit the wsl.config file increase the size to 9GB (you will need it)
- build the .net projects
- in the folders named xxx_db_volume run: docker-compose up -d --build 
- after they run you have initialized all database configurations
- in.net project download all required nugets for the project to build
- in .net projects after build open package manager console and run Update-Database 
(you might have to download nuget of entity frame work tools) this will apply migration to docker volume
- run all docker compose files in the directories python and net
- you might need to update docker env files to hold new exposed address
- run the gateway/authentication project outside docker to call the docker addresses
- test the apis
