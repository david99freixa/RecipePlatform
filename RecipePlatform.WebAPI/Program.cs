using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using RecipePlatform.IoC;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Regista os Repositories, Services e ApplicationContext
builder.Services.AddRecipePlatformServices(
    builder.Configuration);

// Regista os Controllers e o OpenAPI
builder.Services.AddControllers();
builder.Services.AddOpenApi();

// Obtém a chave secreta JWT dos User Secrets
var jwtKey = builder.Configuration["Jwt:Key"]
    ?? throw new InvalidOperationException(
        "A chave JWT não está configurada.");

// Verifica se a chave tem tamanho suficiente
if (Encoding.UTF8.GetByteCount(jwtKey) < 32)
{
    throw new InvalidOperationException(
        "A chave JWT é demasiado curta.");
}

// Identifica quem emite e quem recebe o token
var jwtIssuer = builder.Configuration["Jwt:Issuer"]
    ?? throw new InvalidOperationException(
        "Jwt:Issuer não configurado.");

var jwtAudience = builder.Configuration["Jwt:Audience"]
    ?? throw new InvalidOperationException(
        "Jwt:Audience não configurado.");

// Configura a autenticação através de tokens JWT
builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters =
            new TokenValidationParameters
            {
                // Verifica se o token foi emitido pela nossa API
                ValidateIssuer = true,
                ValidIssuer = jwtIssuer,

                // Verifica se o token se destina à nossa aplicação
                ValidateAudience = true,
                ValidAudience = jwtAudience,

                // Verifica a validade e a assinatura do token
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,

                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(jwtKey)),

                // Não permite tolerância após a expiração
                ClockSkew = TimeSpan.Zero
            };
    });

// Ativa as regras de autorização dos endpoints
builder.Services.AddAuthorization();

var app = builder.Build();

// Disponibiliza a descrição da API em desenvolvimento
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// Identifica o utilizador através do token
app.UseAuthentication();

// Verifica as permissões do utilizador
app.UseAuthorization();

// Ativa as rotas dos Controllers
app.MapControllers();

// Inicia a aplicação
app.Run();
