# Introduction 
TODO: Give a short introduction of your project. Let this section explain the objectives or the motivation behind this project. 

# Getting Started
TODO: Guide users through getting your code up and running on their own system. In this section you can talk about:
1.	Installation process
2.	Software dependencies
3.	Latest releases
4.	API references

# Build and Test
TODO: Describe and show how to build your code and run the tests. 

# Contribute
TODO: Explain how other users and developers can contribute to make your code better. 

If you want to learn more about creating good readme files then refer the following [guidelines](https://docs.microsoft.com/en-us/azure/devops/repos/git/create-a-readme?view=azure-devops). You can also seek inspiration from the below readme files:
- [ASP.NET Core](https://github.com/aspnet/Home)
- [Visual Studio Code](https://github.com/Microsoft/vscode)
- [Chakra Core](https://github.com/Microsoft/ChakraCore)

# Dependencias de Nuget
El proyecto necesita algunas librerias comunes que se encuentran alojado en un repositorio privado de GitHub Packages, la configuracion para acceder a dicho repositorio se encuentra en el archivo de configuracion NuGetDefaults.config, sera necesario copiar el contenido del mismo al archivo ~/.nuget/NuGet/NuGet.Config en Linux o su homologo en Windows, posteriormente se deberan de crear dos variables de ambiente, GITHUB_USER y GITHUB_TOKEN las cuales seran las credenciales de acceso para la descarga de las librerias, como alternativa es posile ejecutar el comando "restore" especificando el archivo de configuracion de nuget ejem. dotnet restore --configfile "./NuGetDefaults.config"