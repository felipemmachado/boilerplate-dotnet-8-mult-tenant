﻿using AutoMapper;
using System.Diagnostics.CodeAnalysis;
namespace Application.Common.Mappings
{
    public interface IMapFrom<T>
    {
        [ExcludeFromCodeCoverage]
        void Mapping(Profile profile)
        {
            profile.CreateMap(typeof(T), GetType());
        }
    }
}
