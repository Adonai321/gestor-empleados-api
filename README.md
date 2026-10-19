#### 1\. Configurar la Cadena de Conexión en **appsettings.json**

Abre tu archivo appsettings.json y asegurese de que la conexión CadenaProveedor apunte al servidor local y a la base de datos correcta PRACTICA\_PROVEEDOR



```

{

&#x20; "ConnectionStrings": {

&#x20;   "CadenaProveedor": "Server=TU\_SERVIDOR\_SQL; Database=PRACTICA\_PROVEEDOR; Integrated Security=True; TrustServerCertificate=True;"

&#x20; }

}

```



#### 2\. Registrar el Servicio en la **API Program.cs**

Verifica que tu clase de datos esté registrada en el contenedor de dependencias antes de la línea var app = builder.Build();



```

// Registrar PeliculaData para que el Controller lo pueda recibir en el constructor

builder.Services.AddScoped<MiWebAPI.Data.PeliculaData>(); 

```

#### 

#### 3\. Ejecutar el Proyecto



Si usas Visual Studio: Presiona la tecla F5 (o el botón verde de "Play" en la barra superior).



Si usas VS Code o Terminal: Abre la consola en la carpeta de tu proyecto (donde está el archivo .csproj) y ejecuta:



```bash

dotnet run

```



#### 4\. Probar la API

Una vez corriendo (normalmente en http://localhost:5299), la interfaz de Swagger se abrirá automáticamente en tu navegador (/swagger). Desde ahí, o importando tu archivo .json en Postman, puedes comenzar a disparar las peticiones.

