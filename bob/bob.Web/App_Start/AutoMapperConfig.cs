using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using bob.Data.Entities;
using bob.Data.DTOs;
using bob.Data.Dictionaries;
using AutoMapper;

namespace bob.App_Start
{
    public static class AutoMapperConfig
    {
        public static void RegisterMappings()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile<HistoriaAcademicaProfile>();
                cfg.AddProfile<PlanEstudioProfile>();              
            });

        }
    }

    public class HistoriaAcademicaProfile : Profile
    {
        public HistoriaAcademicaProfile()
        {
            CreateMap<HistoriaAcademica, AprValue>();
            CreateMap<HistoriaAcademica, CurValue>();
            CreateMap<HistoriaAcademica, PenValue>();
            CreateMap<HistoriaAcademica, NotCurValue>();
        }
    }

    public class PlanEstudioProfile : Profile
    {
        public PlanEstudioProfile()
        {
            CreateMap<PlanEstudio, Materia_Descripcion>();
            CreateMap<PlanEstudio, Correlativa>();
            CreateMap<PlanEstudio, Materia>();
            CreateMap<PlanEstudio, Titulo>().ForMember(dest => dest.Tit_Des, opts => opts.MapFrom(src => src.abr_titulo));
        }
    }
}