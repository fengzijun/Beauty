using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using Beauty.Web.Models;
using Beauty.Model;

namespace Beauty.Web
{
    public class EntityMapper
    {
        #region Singleton Pattern

        private static EntityMapper instance;

        private EntityMapper()
        {
            CreateMap();
        }

        public static EntityMapper Self
        {
            get
            {
                if (instance == null)
                {
                    instance = new EntityMapper();
                }
                return instance;
            }
        }

        #endregion

        public bool IsInstalled { get; set; }

        public void CreateMap()
        {
            if (!IsInstalled)
            {
                //User -> WebMembershipUser
                //Mapper.CreateMap<User, WebMembershipUser>()
                //        .ForMember(dest => dest.PasswordQuestion, opt => opt.MapFrom(src => src.PasswordQuestionId))
                //        .ForMember(dest => dest.PasswordQuestionAnswer, opt => opt.MapFrom(src => src.PasswordAnswer))
                //        .ForMember(dest => dest.LastActivityDate, m => m.MapFrom(src => src.LastActiveDate.HasValue?src.LastActiveDate.Value.ToString("MM/dd/yyyy"):string.Empty))
                //        .ForMember(dest => dest.LastLoginDate, m => m.MapFrom(src => src.LastLoginDate.HasValue ? src.LastLoginDate.Value.ToString("MM/dd/yyyy") : string.Empty))
                //        .ForMember(dest => dest.LastPasswordChangedDate, m => m.MapFrom(src => src.LastPasswordChangedDate.HasValue ? src.LastPasswordChangedDate.Value.ToString("MM/dd/yyyy") : string.Empty))
                //        .ForMember(dest => dest.LastLockoutDate, m => m.MapFrom(src => src.LastLockoutDate.HasValue ? src.LastLockoutDate.Value.ToString("MM/dd/yyyy") : string.Empty));

                Mapper.CreateMap<RegisterModel, User>();
                Mapper.CreateMap<User, RegisterModel>();
                Mapper.CreateMap<BeautyPrice, WebPrice>();
                Mapper.CreateMap<WebPrice, BeautyPrice>();
                Mapper.CreateMap<WebUser, User>().ForMember(dest => dest.Role, opt => opt.MapFrom(src => int.Parse(src.Role)));

                Mapper.CreateMap<User, WebUser>().ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role.ToString())); ;

                IsInstalled = true; //Flag
            }

        }


        /// <summary>
        /// Serialize
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public string Serialize<T>(T t)
        {
            using (StringWriter sw = new StringWriter())
            {
                if (t != null)
                {
                    XmlSerializer xz = new XmlSerializer(t.GetType());
                    xz.Serialize(sw, t);
                    return sw.ToString();
                }
                return null;
            }
        }

        /// <summary>
        /// Deserialize
        /// </summary>
        /// <param name="type"></param>
        /// <param name="s"></param>
        /// <returns></returns>
        public object Deserialize(Type type, string s)
        {
            if (!string.IsNullOrEmpty(s))
            {
                using (StringReader sr = new StringReader(s))
                {
                    XmlSerializer xz = new XmlSerializer(type);
                    return xz.Deserialize(sr);
                }
            }
            return null;
        }

    }
}