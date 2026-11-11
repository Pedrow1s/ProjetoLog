var builder = WebApplication.CreateBuilder(args);

// Configura o console nativo para cuspir JSON estruturado
builder.Logging.ClearProviders();
builder.Logging.AddSimpleConsole(options =>
{
    options.SingleLine = true;
    options.TimestampFormat = "yyyy-MM-dd HH:mm:ss ";
    options.ColorBehavior = Microsoft.Extensions.Logging.Console.LoggerColorBehavior.Disabled;
});

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

//1. Rota para Log de Informação
app.MapPost("/log/info", (ILogger<Program> logger) =>
{
    logger.LogInformation("Ação executada com sucesso. UsuarioId: {UsuarioId}, Acao: {Acao}", 42, "CliqueBotao");
    return Results.Ok(new { status = "Log de Informação emitido!" });
});

//2. Rota para Log de Aviso (Warning)
app.MapPost("/log/aviso", (ILogger<Program> logger) =>
{
    logger.LogWarning("Comportamento suspeito detectado. TentativasRestantes: {Tentativas}", 2);
    return Results.Ok(new { status = "Log de Aviso emitido!" });
});

//3. Rota para Log de Erro ( Error)
app.MapPost("/log/erro", (ILogger<Program> logger) =>
{
    try
    {
        throw new InvalidOperationException("Falha de conexão com serviço externo.");
    }
    catch ( Exception ex)
    {
        logger.LogError(ex, "Erro crítico processado. CodigoErro: {CodigoErro}", 500);
    }
    return Results.Ok(new { status = "Log de Erro emitido!"});
});

app.Run();
