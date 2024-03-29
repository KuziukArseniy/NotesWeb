﻿using AutoMapper;
namespace Notes.Application.Common.Mapping
{
    public interface IMapWith<T>
    {
        public void Mapping(Profile profile) =>
            profile.CreateMap(typeof(T), GetType());
    }
}
