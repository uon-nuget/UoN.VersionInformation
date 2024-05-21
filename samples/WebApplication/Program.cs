var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();


var useEndpointRouting = true; // Whether to use modern endpoint routing or terminal middleware

// TODO DI samples

var objectSource = new
{
    Version = "v1.0.0",
    Description = "You know, for versions."
};

// Pre-endpoint routing terminal middleware
if (!useEndpointRouting)
{
    // Map to the default path `/version`
    // Note that ORDER MATTERS as we are mapping terminally at the same route

    app.UseUonVersionInformation(objectSource);

    // named arguments avoid ambiguity
    // if using a `string` source at the default route
    app.UseUonVersionInformation(source: "v1.0.0");

    app.UseUonVersionInformation(); // Map Assembly version by default


    // Map to custom paths - note that these all work concurrently
    app.UseUonVersionInformation("/custompath"); // Map Assembly version to `/version`
    app.UseUonVersionInformation("/customstring", "Version 1.0");
    app.UseUonVersionInformation("/customobject", objectSource);
}

app.UseRouting(); // We Explicitly opt into endpoint routing here; you may not need to

// Endpoint routing
if (useEndpointRouting)
{
    // Map to the default path `/version`
    // Note that it is not possible to map multiple times to the same endpoint;
    // These will error unless only one is present!
    app.MapUonVersionInformation();
    
    app.MapUonVersionInformation(objectSource);

    // named arguments avoid ambiguity
    // if using a `string` source at the default route
    app.MapUonVersionInformation(source: "v1.0.0");
    
    // Map to custom paths - note that these all work concurrently
    app.MapUonVersionInformation("/custompath"); // Map Assembly version to `/version`
    app.MapUonVersionInformation("/customstring", "Version 1.0");
    app.MapUonVersionInformation("/customobject", objectSource);
}

app.MapGet("/", () => "Hello World!");

app.Run();