using System.Text.Json.Serialization;

namespace AutomationExercise.Tests.ApiClients.Models.Responses;

public sealed class ApiResponse
{
    [JsonPropertyName("responseCode")] public int ResponseCode { get; init; }
    [JsonPropertyName("message")]       public string Message { get; init; } = string.Empty;

    public bool IsSuccess => ResponseCode is 200 or 201;
}

public sealed class ProductListResponse
{
    [JsonPropertyName("responseCode")] public int ResponseCode { get; init; }
    [JsonPropertyName("products")]      public List<ProductDto> Products { get; init; } = [];
}

public sealed class ProductDto
{
    [JsonPropertyName("id")]       public int Id { get; init; }
    [JsonPropertyName("name")]     public string Name { get; init; } = string.Empty;
    [JsonPropertyName("price")]    public string Price { get; init; } = string.Empty;
    [JsonPropertyName("brand")]    public string Brand { get; init; } = string.Empty;
    [JsonPropertyName("category")] public CategoryDto? Category { get; init; }
}

public sealed class CategoryDto
{
    [JsonPropertyName("usertype")] public UserTypeDto? UserType { get; init; }
    [JsonPropertyName("category")] public string CategoryName { get; init; } = string.Empty;
}

public sealed class UserTypeDto
{
    [JsonPropertyName("usertype")] public string UserType { get; init; } = string.Empty;
}

public sealed class BrandListResponse
{
    [JsonPropertyName("responseCode")] public int ResponseCode { get; init; }
    [JsonPropertyName("brands")]        public List<BrandDto> Brands { get; init; } = [];
}

public sealed class BrandDto
{
    [JsonPropertyName("id")]    public int Id { get; init; }
    [JsonPropertyName("brand")] public string Name { get; init; } = string.Empty;
}
