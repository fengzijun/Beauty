using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using System.Xml;
using System.Xml.Serialization;
using Beauty.Model;
using System.IO;
using Beauty.Api.Model;

namespace Beauty.EntityMapper
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

            

                //PaginationInfo - > Pagination
                Mapper.CreateMap<Setting, WebSetting>();
                Mapper.CreateMap<SettingGroup, WebSettingGroup>();
                Mapper.CreateMap<Task, WebTask>();
                Mapper.CreateMap<Bady, WebBady>();
                Mapper.CreateMap<Group, WebGroup>();
                Mapper.CreateMap<Share, WebShare>();
                Mapper.CreateMap<Like, WebLike>();
                Mapper.CreateMap<User, WebUser>();

                Mapper.CreateMap<WebNotice, Notice>();
                Mapper.CreateMap<Notice, WebNotice>();

                IsInstalled = true; //Flag
            }

        }

     

        private void WalletMap()
        {
            //Mapper.CreateMap<Wallet, WebWallet>()
            //    .ForMember(dest => dest.UserID, opt => opt.MapFrom(src => src.Client != null ? src.Client.ToString() : null));
            //Mapper.CreateMap<WebWallet, Wallet>()
            //    .ForMember(dest => dest.Client, opt => opt.MapFrom(src => new Guid(src.UserID)));

            //Mapper.CreateMap<AuditLog, WebAuditLog>()
            //    .ForMember(dest => dest.Level, opt => opt.MapFrom(src => (WebAuditLevel)Enum.Parse(typeof(WebAuditLevel), src.Level)));
            //Mapper.CreateMap<WebAuditLog, AuditLog>()
            //    .ForMember(dest => dest.Level, opt => opt.MapFrom(src => src.Level.ToString()));
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
