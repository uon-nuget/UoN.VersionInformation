var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();


var useEndpointRouting = false; // Whether to use modern endpoint routing or terminal middleware

// TODO DI samples

// Pre-endpoint routing terminal middleware
if (!useEndpointRouting)
{
    // Map to the default path `/version`
    // Note that ORDER MATTERS as we are mapping terminally at the same route
    var objectSource = new
    {
        Version = "v1.0.0",
        Description = "You know, for versions."
    };

    app.UseUonVersion(objectSource);

    // named arguments avoid ambiguity
    // if using a `string` source at the default route
    app.UseUonVersion(source: "v1.0.0");

    app.UseUonVersion(); // Map Assembly version by default


    // Map to custom paths - note that these all work concurrently
    app.UseUonVersion("/custompath"); // Map Assembly version to `/version`
    app.UseUonVersion("/customstring", "Version 1.0");
    app.UseUonVersion("/customobject", objectSource);
}

app.UseRouting(); // We Explicitly opt into endpoint routing here; you may not need to

app.MapGet("/", () => "Hello World!");

app.Run();