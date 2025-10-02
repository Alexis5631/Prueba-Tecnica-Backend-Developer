using AutoMapper;
using Application.DTOs;
using Domain.Entities;

namespace ApiPrueba.Profiles;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        // User mappings
        CreateMap<User, UserDto>()
            .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.Role != null ? src.Role.Name : null))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt.ToDateTime(TimeOnly.MinValue)))
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.UpdatedAt.ToDateTime(TimeOnly.MinValue)))
            .ReverseMap()
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateOnly.FromDateTime(src.CreatedAt)))
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateOnly.FromDateTime(src.UpdatedAt)));

        CreateMap<User, RegisterDto>()
            .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Username))
            .ReverseMap();

        CreateMap<User, LoginDto>()
            .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Username))
            .ReverseMap();

        CreateMap<User, UpdateUserDto>().ReverseMap();

        // Role mappings
        CreateMap<Role, RoleDto>()
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt.ToDateTime(TimeOnly.MinValue)))
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.UpdatedAt.ToDateTime(TimeOnly.MinValue)))
            .ReverseMap()
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateOnly.FromDateTime(src.CreatedAt)))
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateOnly.FromDateTime(src.UpdatedAt)));
        CreateMap<Role, CreateRoleDto>().ReverseMap();
        CreateMap<Role, UpdateRoleDto>().ReverseMap();

        // Product mappings
        CreateMap<Product, ProductDto>()
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt.ToDateTime(TimeOnly.MinValue)))
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.UpdatedAt.ToDateTime(TimeOnly.MinValue)))
            .ReverseMap()
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateOnly.FromDateTime(src.CreatedAt)))
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateOnly.FromDateTime(src.UpdatedAt)));
        CreateMap<Product, CreateProductDto>().ReverseMap();
        CreateMap<Product, UpdateProductDto>().ReverseMap();

        // Order mappings
        CreateMap<Order, OrderDto>()
            .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.User != null ? src.User.Username : null))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt.ToDateTime(TimeOnly.MinValue)))
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.UpdatedAt.ToDateTime(TimeOnly.MinValue)))
            .ReverseMap()
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateOnly.FromDateTime(src.CreatedAt)))
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateOnly.FromDateTime(src.UpdatedAt)));

        CreateMap<Order, CreateOrderDto>().ReverseMap();
        CreateMap<Order, UpdateOrderDto>().ReverseMap();

        // OrderItem mappings
        CreateMap<OrderItem, OrderItemDto>()
            .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product != null ? src.Product.Name : null))
            .ForMember(dest => dest.ProductSku, opt => opt.MapFrom(src => src.Product != null ? src.Product.Sku : null))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt.ToDateTime(TimeOnly.MinValue)))
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.UpdatedAt.ToDateTime(TimeOnly.MinValue)))
            .ReverseMap()
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateOnly.FromDateTime(src.CreatedAt)))
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateOnly.FromDateTime(src.UpdatedAt)));

        CreateMap<OrderItem, CreateOrderItemDto>().ReverseMap();
        CreateMap<OrderItem, UpdateOrderItemDto>().ReverseMap();

        // RefreshToken mappings
        CreateMap<RefreshToken, RefreshTokenDto>()
            .ForMember(dest => dest.RefreshToken, opt => opt.MapFrom(src => src.Token))
            .ReverseMap()
            .ForMember(dest => dest.Token, opt => opt.MapFrom(src => src.RefreshToken));

        // AddRoleDto mapping
        CreateMap<AddRoleDto, User>()
            .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Username))
            .ReverseMap();

        // DataUserDto mapping
        CreateMap<User, DataUserDto>()
            .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Username))
            .ForMember(dest => dest.EstaAutenticado, opt => opt.MapFrom(src => src.IsActive))
            .ForMember(dest => dest.Roles, opt => opt.MapFrom(src => src.Role != null ? new List<string> { src.Role.Name! } : new List<string>()))
            .ReverseMap();
    }
}