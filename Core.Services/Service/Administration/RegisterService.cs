using AutoMapper;
using AutoMapper.QueryableExtensions;
using Core.Data.EF;
using Core.Data.Repository;
using Core.Data.UnitOfWork;
using Core.Infrastruture.Specification;
using Core.Infrastruture.Specification.Implementation;
using Core.Services.DTO.Administration;
using Core.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Infrastructure.Utility;
using Core.Infrastructure.Dev;
using Core.Data.Model;
using System.Collections.Specialized;
using Core.Services.DTO.Generic;
using System.Net.Mail;
using System.IO;
using Microsoft.VisualBasic;
using QRCoder;
using System.Drawing;

namespace Core.Services.Service
{
    public class RegisterService : Repository<MEM_UserActivity, DI_WK_TEMPEntities>, IRegister
    {
        private IMapper _Imapper;

        public RegisterService(WKTempUnitOfWork work, IMapper iImapper)
            : base(work)
        {
            this._Imapper = iImapper;
        }

        #region CRUD methods

        private DevResponse PageData(DevRequest request)
        {
            DevResponse response = new DevResponse();
            IQueryable<RegisterActivityMasterDTO> data =
this.UnitOfWork.CreateSet<MEM_Activity>().UseAsDataSource(this._Imapper).For<RegisterActivityMasterDTO>();

            Specification<RegisterActivityMasterDTO> specs = base.DevGridFilters<RegisterActivityMasterDTO>(request.Filters);
            var filteredData = data.Where(specs.SatisfiedBy());

            if (request.Sorts.Count > 0)
            {
                foreach (var sort in request.Sorts)
                {
                    filteredData = filteredData.OrderBy<RegisterActivityMasterDTO>(sort.fieldname, sort.desc);
                }
            }
            else
            {
                filteredData = filteredData.OrderByDescending(orderby => orderby.startDate);
            }

            //var dataPage = filteredData.Skip(request.Skip).Take(request.Take);
            IQueryable<RegisterActivityMasterDTO> dataPage;
            if (!request.RequireTotalCount && request.Take <= 0)
                dataPage = filteredData;
            else
                dataPage = filteredData.Skip(request.Skip).Take(request.Take);

            response.data = dataPage.ToList();
            if (request.RequireTotalCount)
                response.totalCount = filteredData.Count();
            return response;
        }

        private DevResponse EditByQR(DevRequest request, LogModel logmodel)
        {
            DevResponse response = new DevResponse();
            RegisterActivityMasterDTO orgin = base.ConvertObject<RegisterActivityMasterDTO>(request.Values);

            Guid guid = Guid.Empty;
            Guid.TryParse(request.Key, out guid);

            MEM_UserActivity convert_hanlder = this.GetByID(guid);
            if (convert_hanlder != null)
            {
                convert_hanlder.UAC_AttDate = orgin.UAC_AttDate;
                convert_hanlder.UAC_UpdateDate = DateTime.Now;
                convert_hanlder.UAC_UpdateUser = logmodel.Insert_User;

                var result_update = base.Update(convert_hanlder);
                if (result_update.Code == 0)
                {
                    response.haveError = false;
                    response.key = request.Key;
                    response.data = orgin;
                }
                else
                {
                    response.haveError = true;
                    response.error = "CODE:" + result_update.Code + "  MSG:" + result_update.ErrMsg;
                }
            }

            //logmodel.PK = System.Guid.NewGuid();
            //logmodel.Details = this.UnitOfWork.Sql;
            //logmodel.Action = request.CuttentAction;
            //logmodel.Status = !response.haveError;
            //logmodel.Remark = response.error;
            //base.Log(logmodel);

            return response;
        }

        private DevResponse EditByPhone(DevRequest request, LogModel logmodel)
        {
            DevResponse response = new DevResponse();
            RegisterActivityMasterDTO orgin = base.ConvertObject<RegisterActivityMasterDTO>(request.Values);

            IEnumerable<MEM_UserActivity> user_activities = this.GetAll().Where(u => u.UAC_ACT_PK == orgin.ACT_PK && u.UAC_MBR_PK == orgin.MBR_PK).ToList();
            
            foreach(var user_activity in user_activities)
            {
                user_activity.UAC_AttDate = orgin.UAC_AttDate;
                user_activity.UAC_UpdateDate = DateTime.Now;
                user_activity.UAC_UpdateUser = logmodel.Insert_User;

                var result_update = base.Update(user_activity);
                if (result_update.Code == 0)
                {
                    response.haveError = false;
                    response.key = request.Key;
                    response.data = orgin;
                }
                else
                {
                    response.haveError = true;
                    response.error = "CODE:" + result_update.Code + "  MSG:" + result_update.ErrMsg;
                    break;
                }
            }

            
            //logmodel.PK = System.Guid.NewGuid();
            //logmodel.Details = this.UnitOfWork.Sql;
            //logmodel.Action = request.CuttentAction;
            //logmodel.Status = !response.haveError;
            //logmodel.Remark = response.error;
            //base.Log(logmodel);

            return response;
        }

        private DevResponse SingleDataByPhone(DevRequest request)
        {
            DevResponse response = new DevResponse();
            RegisterActivityMasterDTO orgin = base.ConvertObject<RegisterActivityMasterDTO>(request.Values);
            
            IQueryable<MemberDTO> member_data =
this.UnitOfWork.CreateSet<MEM_Membership>().UseAsDataSource(this._Imapper).For<MemberDTO>();
            IQueryable<UserActivityDTO> uac_data = this.GetAll().UseAsDataSource(this._Imapper).For<UserActivityDTO>()
                                                    .Where(uac => uac.UAC_ACT_PK == orgin.ACT_PK);
            var singledata = member_data.Where(predicate => predicate.MBR_Phone1 == request.Key || predicate.MBR_Phone2 == request.Key).FirstOrDefault();
            Guid mbr_pk = singledata == null ? Guid.Empty : singledata.MBR_PK;

            if (singledata != null)
            {
                var uac_singledata = uac_data.Where(uac => uac.UAC_MBR_PK == mbr_pk);
                if (uac_singledata.Count() > 0)
                {
                    orgin.MBR_PK = singledata?.MBR_PK;
                    orgin.MBR_Name = singledata?.MBR_Name;
                    orgin.MBR_Phone1 = singledata?.MBR_Phone1;
                    orgin.UAC_RegDate = uac_singledata.Max(u => u.UAC_RegDate);
                    //reset two temp fielddata for input text box
                    orgin.Flex2 = null;
                    orgin.Flex1 = null;

                    response.data = orgin;
                }
                else
                {
                    response.data = null;
                    response.error = "No corresponding UserActivity found";
                }
            }
            else
            {
                response.data = null;
                response.error = "Phone number not found";
            }

            

            response.key = request.Key;

            return response;
        }

        private DevResponse SingleDataByQR(DevRequest request)
        {
            DevResponse response = new DevResponse();
            RegisterActivityMasterDTO orgin = base.ConvertObject<RegisterActivityMasterDTO>(request.Values);
            Guid uac_pk = Guid.Empty;
            Guid mbr_pk = Guid.Empty;
            Guid.TryParse(request.Key, out uac_pk);

            IQueryable<MemberDTO> member_data =
this.UnitOfWork.CreateSet<MEM_Membership>().UseAsDataSource(this._Imapper).For<MemberDTO>();
            MEM_UserActivity uac_data = this.GetByID(uac_pk);
            if (uac_data != null && uac_data.UAC_ACT_PK == orgin.ACT_PK)
                mbr_pk = uac_data.UAC_MBR_PK;

            var singledata = member_data.Where(predicate => predicate.MBR_PK == mbr_pk).FirstOrDefault();
            
            orgin.MBR_PK = singledata?.MBR_PK;
            orgin.MBR_Name = singledata?.MBR_Name;
            orgin.MBR_Phone1 = singledata?.MBR_Phone1;
            orgin.UAC_RegDate = singledata == null ? orgin.UAC_RegDate : uac_data.UAC_RegDate;
            //reset two temp fielddata for input text box
            orgin.Flex2 = null;
            orgin.Flex1 = null;

            response.data = singledata == null ? null : orgin;
            response.key = request.Key;

            return response;
        }


        #endregion


        public DevResponse DevPageAll(NameValueCollection request, LogModel logmodel = null)
        {
            DevRequest convertRequest = new DevRequest(request);
            switch (convertRequest.CuttentAction)
            {
                case "search":
                    return this.PageData(convertRequest);
                case "searchsingle":
                    throw new NotImplementedException(); //return this.SingleData(convertRequest);
                case "delete":
                    throw new NotImplementedException();//return this.Remove(convertRequest, logmodel);
                case "updatebyqr":
                    return this.EditByQR(convertRequest, logmodel);
                case "updatebyphone":
                    return this.EditByPhone(convertRequest, logmodel);
                case "searchphone":
                    return this.SingleDataByPhone(convertRequest);
                case "searchqr":
                    return this.SingleDataByQR(convertRequest);
                default:
                    throw new NotImplementedException();
            }
        }

        protected Task SendFormattedHTMLEmailAsync(HTMLEmailViewModel model, Stream stream)
        {
            string setting_address = System.Configuration.ConfigurationManager.AppSettings["MailAddress"];
            string setting_smtp = System.Configuration.ConfigurationManager.AppSettings["MailSMTP"];
            int setting_port = Convert.ToInt16(System.Configuration.ConfigurationManager.AppSettings["MailPort"]);
            string setting_pass = System.Configuration.ConfigurationManager.AppSettings["MailPass"];

            MailMessage msg = new MailMessage();
            msg.From = new MailAddress(setting_address);

            msg.Subject = model.Subject;
            msg.Body = model.Body;
            msg.IsBodyHtml = true;
            msg.To.Add(new MailAddress(model.Destination));
            stream.Seek(0, SeekOrigin.Begin);
            Attachment attachment = new Attachment(stream, System.Net.Mime.MediaTypeNames.Image.Jpeg);
            attachment.ContentDisposition.FileName = "Ticket.jpeg";
            msg.Attachments.Add(attachment);

            SmtpClient smtpClient = new SmtpClient(setting_smtp);
            smtpClient.EnableSsl = true;
            System.Net.NetworkCredential credentials = new System.Net.NetworkCredential(setting_address, setting_pass);
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = credentials;
            smtpClient.Port = setting_port;

            return smtpClient.SendMailAsync(msg);
        }

        public async Task<DevResponse> RegisterNewOrExistingMember(NameValueCollection request, LogModel logmodel, object emailModel = null)
        {
            DevRequest converted_request = new DevRequest(request);
            DevResponse response = new DevResponse();
            RegisterActivityNewMemberDTO new_member = base.ConvertObject<RegisterActivityNewMemberDTO>(converted_request.Values);


            Guid new_member_pk = Guid.NewGuid();
            MEM_Membership member_entity = new MEM_Membership()
            {
                MBR_PK = new_member_pk,
                MBR_Type = 1,
                MBR_Name = new_member.MBR_Name,
                MBR_Phone1 = new_member.MBR_Phone1,
                MBR_Agreement = true,
                MBR_IsEnable = true,
                MBR_Email = new_member.MBR_Email,
                MBR_InsertDate = DateTime.Now,
                MBR_InsertUser = logmodel.Insert_User
            };

            this.UnitOfWork.CreateSet<MEM_Membership>().Add(member_entity);
            if (this.UnitOfWork.Commit() < 1)
            {
                response.haveError = true;
                response.error = "MSG: save fail";
            }
            else
            {
                response.haveError = false;
                response.key = new_member_pk.ToString();
                //response.data = member_entity;
            }


            if (!response.haveError && new_member.IsCheckin)
            {
                ///////////////////////
                Guid act_pk = new_member.ACT_PK;
                var activity = this.UnitOfWork.CreateSet<MEM_Activity>()
                        .UseAsDataSource(this._Imapper).For<ActivityDTO>()
                        .Where(x => x.ACT_PK == act_pk)
                        .SingleOrDefault();

                Guid new_uac_pk = Guid.NewGuid();
                MEM_UserActivity uac_entity = new MEM_UserActivity()
                {
                    UAC_PK = new_uac_pk,
                    UAC_MBR_PK = new_member_pk,
                    UAC_ACT_PK = activity.ACT_PK,
                    UAC_ACT_Name = activity.ACT_Name,
                    UAC_ACT_Type = activity.ACT_Type,
                    UAC_ACT_From_Date = activity.ACT_FromDate,
                    UAC_ACT_To_Date = activity.ACT_ToDate,
                    UAC_RegDate = DateTime.Now,
                    UAC_AttDate = DateTime.Now,
                    UAC_Current = activity.ACT_Current,
                    UAC_Fee = activity.ACT_Fee,
                    UAC_Remarks = new_member.MBR_Email,
                    UAC_InsertDate = DateTime.Now,
                    UAC_InsertUser = logmodel.Insert_User
                };


                var result_add = base.Add(uac_entity);
                if (result_add.Code == 0)
                {
                    try
                    {
                        //Email ?
                        if (new_member.IsEmail)
                        {
                            if (emailModel == null)
                            {
                                response.haveError = true;
                                response.error = "MSG: emailModel is empty!";
                            }
                            else
                            {
                                //converted_handler_list.ToList().ForEach(address =>
                                //{
                                var email_model = (HTMLEmailViewModel)emailModel;
                                QRCodeGenerator qrGenerator = new QRCodeGenerator();
                                QRCodeData qrCodeData = qrGenerator.CreateQrCode(new_uac_pk.ToString(), QRCodeGenerator.ECCLevel.Q);
                                QRCode qrCode = new QRCode(qrCodeData);
                                using (Bitmap bitMap = qrCode.GetGraphic(20))
                                {
                                    using (MemoryStream ms = new MemoryStream())
                                    {
                                        //try
                                        //{ 
                                        bitMap.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);

                                        email_model.Body = email_model.Body.Replace("{COURSESTARTDATE}", activity.ACT_FromDate.Value.ToString());
                                        email_model.Subject = "Ticket to Course: " + activity.ACT_Name;
                                        email_model.Destination = new_member.MBR_Email;

                                        //send email
                                        await this.SendFormattedHTMLEmailAsync(email_model, ms);
                                    }
                                }

                                response.haveError = false;
                                response.key = new_uac_pk.ToString();
                                //response.data = uac_entity;

                            }
                        }
                        else
                        {
                            response.haveError = false;
                            response.key = new_uac_pk.ToString();
                            //response.data = uac_entity;
                        }
                    }
                    catch (Exception mail_ex)
                    {
                        logmodel.Action = "error";
                        logmodel.Remark = mail_ex.Message;
                        base.Log(logmodel);
                    }
                }
                else
                {
                    response.haveError = true;
                    response.error = "CODE:" + result_add.Code + "  MSG:" + result_add.ErrMsg;
                }

                ////////////////////////////////////
            }

            //logmodel.PK = System.Guid.NewGuid();
            //logmodel.Details = this.UnitOfWork.Sql;
            //logmodel.Action = request.CuttentAction;
            //logmodel.Status = !response.haveError;
            //logmodel.Remark = response.error;
            //base.Log(logmodel);

            return response;
        }

        public async Task<DevResponse> UploadForm(byte[] filebinary, Core.Data.Model.LogModel logmodel, bool isEmail = true, object emailModel = null)
        {
            DevResponse response = new DevResponse();
            MemoryStream memoryStream = new MemoryStream(filebinary);

            List<GoogleFormDTO> forms = new List<GoogleFormDTO>();
            using (var parser = new Microsoft.VisualBasic.FileIO.TextFieldParser(memoryStream))
            {
                parser.HasFieldsEnclosedInQuotes = true;
                parser.Delimiters = new[] { "," };

                var list = new List<string[]>();
                while (parser.PeekChars(1) != null)
                {
                    string[] fields = parser.ReadFields();
                    list.Add(fields);
                }

                var query = from f in list
                            select new GoogleFormDTO
                            {
                                A = f[0],
                                B = f[1],
                                C = f[2],
                                D = f[3],
                                E = f[4],
                                F = f[5],
                            };

                forms = query.Skip(1).ToList();
            }

            //var members = this.UnitOfWork.CreateSet<MEM_Membership>().UseAsDataSource(this._Imapper).For<MemberDTO>();
            var members = this.UnitOfWork.CreateSet<MEM_Membership>().AsQueryable();
            var activities = this.UnitOfWork.CreateSet<MEM_Activity>().UseAsDataSource(this._Imapper).For<ActivityDTO>();

            System.Globalization.CultureInfo provider = System.Globalization.CultureInfo.InvariantCulture;
            var user_activities = from form in forms
                                    from member in members
                                    .Where(member => member.MBR_Phone1 == form.D || member.MBR_Phone2 == form.D)
                                    from activity in activities
                                    .Where(activity => form.F.Contains(activity.ACT_Name))
                                    select new MEM_UserActivity //UserActivityDTO
                                    {
                                        UAC_PK = Guid.NewGuid(),
                                        UAC_MBR_PK = member.MBR_PK,
                                        UAC_ACT_PK = activity.ACT_PK,
                                        UAC_ACT_Name = activity.ACT_Name,
                                        UAC_ACT_Type = activity.ACT_Type,
                                        UAC_ACT_From_Date = activity.ACT_FromDate,
                                        UAC_ACT_To_Date = activity.ACT_ToDate,
                                        UAC_RegDate = DateTime.ParseExact(form.A, "yyyy/MM/dd h:mm:ss tt 'GMT+8'", provider),
                                        UAC_Current = activity.ACT_Current,
                                        UAC_Fee = activity.ACT_Fee,
                                        UAC_Remarks = form.B,
                                        //UAC_MBR_Phone = form.D,
                                        //UAC_ACT_ID = activity.ACT_ID,
                                        UAC_InsertDate = DateTime.Now,
                                        UAC_InsertUser = logmodel.Insert_User //Identity.User
                                    };
            //IEnumerable<MEM_UserActivity> converted_handler = this._Imapper.Map<IEnumerable<MEM_UserActivity>>(user_activities);
            List<MEM_UserActivity> converted_handler_list = user_activities.ToList();

            var result_add = base.AddRange(converted_handler_list);
            if (result_add.Code == 0)
            {
                //Email ?
                if (isEmail)
                {
                    if (emailModel == null)
                    {
                        response.haveError = true;
                        response.error = "MSG: emailModel is empty!";
                    }
                    else
                    {
                        //Imgur's API
                        //try
                        //{
                        //var imgur_client = new Imgur.API.Authentication.Impl.ImgurClient("1ddca48bbbf58b0", "d1d15c74a6d0d1ad9d42d1befb56827a62d025a4");
                        //var imgur_endpoint = new Imgur.API.Endpoints.Impl.ImageEndpoint(imgur_client);
                        //imgur_endpoint.HttpClient.BaseAddress = new Uri("https://api.imgur.com/3/image");

                        foreach (var address in converted_handler_list)
                        {
                            //converted_handler_list.ToList().ForEach(address =>
                            //{
                            var email_model = (HTMLEmailViewModel)emailModel;
                            QRCodeGenerator qrGenerator = new QRCodeGenerator();
                            QRCodeData qrCodeData = qrGenerator.CreateQrCode(address.UAC_PK.ToString(), QRCodeGenerator.ECCLevel.Q);
                            QRCode qrCode = new QRCode(qrCodeData);
                            using (Bitmap bitMap = qrCode.GetGraphic(20))
                            {
                                using (MemoryStream ms = new MemoryStream())
                                {
                                    //try
                                    //{ 
                                    bitMap.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);

                                    //Imgur.API.Models.IImage image = imgur_endpoint.UploadImageStreamAsync(ms).GetAwaiter().GetResult();
                                    // email_model.Body = email_model.Body.Replace("{QR_Code}", image.Link);
                                    email_model.Body = email_model.Body.Replace("{COURSESTARTDATE}", address.UAC_ACT_From_Date.Value.ToString());
                                    email_model.Subject = "Ticket to Course: " + address.UAC_ACT_Name;
                                    email_model.Destination = address.UAC_Remarks;

                                    //send email
                                    await this.SendFormattedHTMLEmailAsync(email_model, ms);
                                    //}
                                    //    catch (Imgur.API.ImgurException imgurEx)
                                    //    {

                                    //    }
                                    //    catch (Exception ex1)
                                    //{ }
                                }
                            }

                            //});
                        }


                        //}
                        //catch (Exception e)
                        //{ }

                        response.haveError = false;
                        response.data = converted_handler_list;

                    }
                }
                else
                {
                    response.haveError = false;
                    response.data = converted_handler_list; //request.Values;
                }
            }
            else
            {
                response.haveError = true;
                response.error = "CODE:" + result_add.Code + "  MSG:" + result_add.ErrMsg;
            }


            return response;
        }

        public DevResponse LoadDetailGrid(string act_id)
        {
            DevResponse response = new DevResponse();
            Guid act_pk = Guid.Empty;
            Guid.TryParse(act_id, out act_pk);

            IQueryable<MEM_UserActivity> uacs = base.GetFiltered(x => x.UAC_ACT_PK == act_pk);
            IQueryable<MEM_Membership> members = this.UnitOfWork.CreateSet<MEM_Membership>().AsQueryable();
            var result_data = from uac in uacs
                              from member in members
                                .Where(member => member.MBR_PK == uac.UAC_MBR_PK)
                              select new RegisterActivityGridDTO
                              {
                                  ACT_PK = uac.UAC_ACT_PK,
                                  ACT_Name = uac.UAC_ACT_Name,
                                  Remark = uac.UAC_Remarks,
                                  MBR_Name = member.MBR_Name,
                                  MBR_ChineseName = member.MBR_ChineseName,
                                  MBR_Phone1 = member.MBR_Phone1,
                                  MBR_Phone2 = member.MBR_Phone2,
                                  MBR_WeChatNo = member.MBR_WeChatNo,
                                  RegDate = uac.UAC_RegDate
                              };

            List<RegisterActivityGridDTO> final_data_list =
                result_data.ToList();
            
            if (final_data_list.Count() > 0)
            {
                response.haveError = false;
                response.key = final_data_list.First().ACT_Name;
                response.data = final_data_list;
            }
            else
            {
                response.haveError = true;
                response.error = "No joiners";
            }

            return response;
        }
    }
}
