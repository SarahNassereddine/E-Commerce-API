
using lab3.Data;
using lab3.Models;
using lab3.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
// ① Load connection string from appsettings.json
var connString = builder.Configuration
    .GetConnectionString("DefaultConnection");
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ② Register AppDbContext with DI (scoped lifetime)
builder.Services.AddDbContext<AppDbContext>(
    // ③ Choose database provider — swap for UseSqlServer, UseNpgsql…
    options => options.UseSqlServer(connString));
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped< IProductService, ProductService>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

//using (var scope = app.Services.CreateScope())
//{

//    var services = scope.ServiceProvider;
//    var context = services.GetRequiredService<AppDbContext>();
//    await context.Database.EnsureDeletedAsync();
//    await context.Database.EnsureCreatedAsync();
//    var productService = services.GetRequiredService<IProductService>();
//    var categoryService = services.GetRequiredService<ICategoryService>();
//    await Handler.insertDummyData(productService, categoryService);
//}

var productRoutes = app.MapGroup("/products")
    
    .WithOpenApi()
    .WithTags("Products");

var categoryRoutes = app.MapGroup("/categories")
    
    .WithOpenApi()
    .WithTags("Categories");

productRoutes.MapGet("/{id}", Handler.getProduct).WithName("view-product")
    .WithSummary("get product by id")
    .Produces(StatusCodes.Status200OK)
    .Produces(StatusCodes.Status404NotFound);

productRoutes.MapGet("/", Handler.getAllProducts)
      .WithSummary("get all products")
    .Produces(StatusCodes.Status200OK)
    .Produces(StatusCodes.Status404NotFound);

productRoutes.MapPut("/", Handler.createProduct)
    .WithSummary("create product")
    .Produces(StatusCodes.Status201Created)
    .Produces(StatusCodes.Status404NotFound);

productRoutes.MapPost("/{id}", Handler.updateProduct)
    .WithSummary("update a product")
    .Produces(StatusCodes.Status200OK)
    .Produces(StatusCodes.Status404NotFound);

productRoutes.MapDelete("/{id}", Handler.deleteProduct)
    .WithSummary("delete a product")
    .Produces(StatusCodes.Status200OK);



categoryRoutes.MapGet("/{id}", Handler.getCategory).WithName("view-category")
    .WithSummary("get category by id")
    .Produces(StatusCodes.Status200OK)
    .Produces(StatusCodes.Status404NotFound);

categoryRoutes.MapGet("/", Handler.getAllCategories)
      .WithSummary("get all categories")
    .Produces(StatusCodes.Status200OK)
    .Produces(StatusCodes.Status404NotFound);

categoryRoutes.MapPut("/", Handler.createCategory)
    .WithSummary("create category")
    .Produces(StatusCodes.Status201Created);

categoryRoutes.MapPost("/{id}", Handler.updateCategory)
    .WithSummary("update a category")
    .Produces(StatusCodes.Status200OK)
    .Produces(StatusCodes.Status404NotFound);

categoryRoutes.MapDelete("/{id}", Handler.deleteCategory)
    .WithSummary("delete a category")
    .Produces(StatusCodes.Status200OK);




app.Run();

public class Handler
{
    public static async Task<IResult> getProduct( int id, IProductService service)
    {
        var product = await service.ReadProduct(id);
        if(product == null)
        {
          return Results.NotFound("no product with such id");

        }
         return Results.Ok(product);
    }
    public static async Task<IResult> getCategory(int id, ICategoryService service)
    {
        var category = await service.ReadCategory(id);
        if (category == null)
        {
            return Results.NotFound("no product with such id");

        }
        return Results.Ok(category);
    }
    public static async Task<IResult> getAllProducts(IProductService service)
    {
        var products = await service.ReadProducts();
        return Results.Ok(products);
    }
    public static async Task<IResult> getAllCategories(ICategoryService service)
    {
        var products = await service.ReadCategories();
        return Results.Ok(products);
    }
    public static async Task<IResult> createProduct(CreateProductCmd cmd, IProductService service)
    {
        try
        {
            var productId = await service.CreateProduct(cmd);
            return Results.CreatedAtRoute("view-product", new { id=productId }, productId);
        }
        catch (KeyNotFoundException e)
        {
            return Results.NotFound(e.Message);
        }
    }
    public static async Task<IResult> createCategory(CreateCategoryCmd cmd, ICategoryService service)
    {
          var categoryId = await service.CreateCategory(cmd);
            return Results.CreatedAtRoute("view-category", new { id=categoryId }, categoryId);
    }

    public static async Task<IResult> deleteProduct(IProductService service, int id)
    {
        await service.DeleteProduct(id);
        return Results.Ok("product succefully deleted.");

    }
    public static async Task<IResult> deleteCategory(ICategoryService service, int id)
    {
        await service.DeleteCategory(id);
        return Results.Ok("category succefully deleted.");

    }
    public static async Task<IResult> updateProduct(UpdateProductCmd cmd, IProductService service, int id)
    {
        try
        {
            await service.UpdateProduct(cmd, id);
            return Results.Ok("product updated");
        }
        catch (Exception e) { 
        return Results.NotFound(e.Message);
        }
    }
    public static async Task<IResult> updateCategory(UpdateCategoryCmd cmd, ICategoryService service, int id)
    {
        try
        {
            await service.UpdateCategory(cmd, id);
            return Results.Ok("category updated");
        }
        catch (Exception e)
        {
            return Results.NotFound(e.Message);
        }
    }
    public async static Task insertDummyData(IProductService productService, ICategoryService categoryService)
    {
        await categoryService.CreateCategory(new CreateCategoryCmd
        {
            Name = "c1"
        });
        await categoryService.CreateCategory(new CreateCategoryCmd
        {
            Name = "c2"
        });
        await categoryService.CreateCategory(new CreateCategoryCmd
        {
            Name = "c3"
        });
        await productService.CreateProduct(new CreateProductCmd
        {
            Name="p1",
            Description="d1",
            CategoryId=1,
            Price=10
          
        });
        await productService.CreateProduct(new CreateProductCmd
        {
            Name = "p2",
            Description = "d2",
            CategoryId = 1,
            Price = 10

        });
        await productService.CreateProduct(new CreateProductCmd
        {
            Name = "p1",
            CategoryId = 1,
            Price = 10

        });
    }
   
}