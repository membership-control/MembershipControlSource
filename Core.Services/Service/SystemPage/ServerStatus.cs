using AutoMapper;
using AutoMapper.QueryableExtensions;
using Core.Data.EF;
using Core.Data.Repository;
using Core.Data.UnitOfWork;
using Core.Infrastruture.Specification;
using Core.Infrastruture.Specification.Implementation;
using Core.Services.DTO.SystemPage;
using Core.Services.Interface;
using DataTables.AspNet.Core;
using DataTables.AspNet.Mvc5;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Infrastructure.Utility;
using Core.Infrastructure.DataTables;
using Newtonsoft.Json;
using Core.Infrastructure.Specification;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Core.Identity;
using Core.Identity.Models;
using Core.Infrastructure.Dev;
using Core.Data.Model;

namespace Core.Services.Service
{
    public class ServerStatus : Repository<TGF_GI_ControlTower_Server_Status_Setting, TGF_IntegrationEntities>, IServerStatus
    {
                private IMapper _Imapper;

                public ServerStatus(IntegrationUnitOfWork work, IMapper iImapper)
            : base(work)
        {
            this._Imapper = iImapper;
        }
                public List<ServerDetailDTO> GetAllData()
                {
                    using (TGF_IntegrationEntities db = new TGF_IntegrationEntities())
                    {
                        return (from a in db.TGF_GI_ControlTower_Server_Status_Setting
                                where a.IsEnabled == true 
                                select new ServerDetailDTO
                                {
                                    Server_Name = a.Server_Name
                                }).Distinct().ToList();
                    }
                }
    }
}
