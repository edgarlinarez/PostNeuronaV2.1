using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// La información general sobre un ensamblado se controla mediante lo siguiente
// conjunto de atributos. Cambie los valores de estos atributos para modificar la información
// asociada con un ensamblado.
[assembly: AssemblyTitle("POSTNeurona")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("")]
[assembly: AssemblyProduct("POSTNeurona")]
[assembly: AssemblyCopyright("Copyright ©  2022")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Si configura ComVisible como falso, los tipos de este ensamblado no se hacen visibles
// para componentes COM.  Si necesita acceder a un tipo de este ensamblado desde
// COM, establezca el atributo ComVisible en True en este tipo.
[assembly: ComVisible(false)]

// El siguiente GUID sirve como ID de typelib si este proyecto se expone a COM
[assembly: Guid("7ba8b937-24dd-4c62-a1fb-47c052fb9b3b")]

// La información de versión de un ensamblado consta de los siguientes cuatro valores:
//
//      Versión principal
//      Versión secundaria
//      Número de compilación
//      Revisión
//
// Puede especificar todos los valores o puede predeterminar los números de compilación y de revisión
// mediante el carácter '*', como se muestra a continuación:
[assembly: AssemblyVersion("1.0.0.0")]
[assembly: AssemblyFileVersion("1.0.0.0")]
//log4net
[assembly: log4net.Config.XmlConfigurator(ConfigFile = "log4net.config")]