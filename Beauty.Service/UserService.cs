using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Beauty.Service
{
    using Beauty.Core;
    using Beauty.InterFace;
    using Beauty.Model;

    public class UserService : BaseService, IUser
    {
        #region private function

        /// <summary>
        /// Don't repeat yourself! This private function is for Userd by all query public functions
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="page"></param>
        /// <param name="pg"></param>
        /// <returns></returns>
        IList<User> GetUsers(string sql, int page, string sortKey, out PaginationInfo paging)
        {
            using (DataSet ds = SqlHelper.ExecuteDataset(ConnectStr, CommandType.Text, sql))
            {
                if (ds == null || ds.Tables.Count != 2)
                {
                    throw new Exception("SQL execution failed");
                }
                else
                {
                    List<User> Comments = new List<User>();

                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        User Comment = new User()
                        {
                            ID = new Guid(dr["id"].ToString()),
                            Availtime = ParseNDate(dr["Availtime"].ToString()),
                            Bank = ParseStr(dr["Bank"]),
                            BeautyAccount = ParseStr(dr["BeautyAccount"]),
                            BeautyPassword = ParseStr(dr["BeautyPassword"]),
                            Card = ParseStr(dr["Card"]),
                            City = ParseStr(dr["City"]),
                            Email = ParseStr(dr["Email"]),
                            Ip = ParseStr(dr["Ip"]),
                            IsLogin = bool.Parse(dr["IsLogin"].ToString()),
                            IsSuper = bool.Parse(dr["IsSuper"].ToString()),
                            Lastlogintime = ParseDate(dr["Lastlogintime"]),
                            Liked = ParseInt(dr["Liked"].ToString()),
                            Mobile = ParseStr(dr["Mobile"]),
                            Password = ParseStr(dr["Password"]),
                            Point = ParseDecimal(dr["Point"]),
                            FreezePoint = ParseDecimal(dr["FreezePoint"]),
                            
                            Province = ParseStr(dr["Province"]),
                            QQ = ParseStr(dr["QQ"]),
                            Balance = ParseDecimal(dr["Balance"]),
                            Rate = ParseStr(dr["Rate"]),
                            Refer = ParseStr(dr["Refer"]),
                            MaxPoint = ParseNDecimal(dr["MaxPoint"]),
                            TimePoint = ParseNDecimal(dr["TimePoint"]),
                            Role = ParseInt(dr["Role"]),
                            ShopAddress = ParseStr(dr["ShopAddress"]),
                            ZFB = ParseStr(dr["ZFB"]),
                            Username = ParseStr(dr["Username"]),
                            Statues = ParseInt(dr["Statues"]),
                            Createby = ParseStr(dr["Createby"]),
                            Createtime = ParseStr(dr["Createtime"]),
                            Updateby = ParseStr(dr["Updateby"]),
                            Updatetime = ParseStr(dr["Updatetime"])
                        };

                        Comments.Add(Comment);
                    }


                    paging = new PaginationInfo()
                    {
                        Current = page,
                        Size = ParseInt(ds.Tables[1].Rows[0]["pagesize"]),
                        TotalRecords = ParseInt(ds.Tables[1].Rows[0]["totalrecords"]),
                        TotalPages = (int)Math.Ceiling(ParseInt(ds.Tables[1].Rows[0]["totalrecords"]) /
                        ParseFloat(ds.Tables[1].Rows[0]["pagesize"]))
                    };

                    return Comments;
                }
            }
        }

        #endregion

        #region * CRUD *

        /// <summary>
        /// Create a new User
        /// </summary>
        /// <param name="newUser">new User</param>
        /// <returns>new User id</returns>
        public Guid Create(User newUser)
        {
            string sql = string.Format("EXEC sp_Users_c {0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17},{18},{19},{20},{21},{22},{23},{24},{25},{26},{27},{28},{29},{30},{31}"
                                         , ToQuote(newUser.ID)
                                         , ToQuote(newUser.Username)
                                         , ToQuote(newUser.Email)
                                         , ToQuote(newUser.QQ)
                                         , ToQuote(newUser.Mobile)
                                         , ToQuote(newUser.IsSuper)
                                         , ToQuote(newUser.Liked)
                                         , ToQuote(newUser.Password)
                                         , ToQuote(newUser.Province)
                                         , ToQuote(newUser.City)
                                         , ToQuote(newUser.ShopAddress)
                                         , ToQuote(newUser.Availtime)
                                         , ToQuote(newUser.BeautyAccount)
                                         , ToQuote(newUser.BeautyPassword)
                                         , ToQuote(newUser.Refer)
                                         , ToQuote(newUser.Point)
                                         , ToQuote(newUser.Balance)
                                         , ToQuote(newUser.Role)
                                         , ToQuote(newUser.ZFB)
                                         , ToQuote(newUser.Card)
                                         , ToQuote(newUser.IsLogin)
                                         , ToQuote(newUser.Ip)
                                         , ToQuote(newUser.Lastlogintime)
                                         , ToQuote(newUser.Bank)
                                         , ToQuote(newUser.Rate)
                                         , newUser.TimePoint.HasValue? ToQuote(newUser.TimePoint.Value):"NULL"
                                         , newUser.MaxPoint.HasValue ? ToQuote(newUser.MaxPoint.Value) : "NULL"
                                         , ToQuote(CurrentUserName)
                                         , ToQuote(DateTime.Now)
                                         , ToQuote(CurrentUserName)
                                         , ToQuote(DateTime.Now)
                                         , ToQuote(newUser.Statues)

                                     );

            try
            {
                int rowcount = SqlHelper.ExecuteNonQuery(ConnectStr, CommandType.Text, sql);

                if (rowcount == 1)
                {
                    return newUser.ID;
                }
                else
                {
                    throw new Exception("SQL execution failed");
                }

            }
            catch (Exception ex)
            {
                throw new Exception("SQL execution failed", ex);
            }
        }
        /// <summary>
        /// Update an existing User
        /// </summary>
        /// <param name="thisUser">User</param>
        /// <returns>bool</returns>
        public bool Update(User thisUser)
        {
            string sql = string.Format("EXEC sp_Users_u {0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17},{18},{19},{20},{21},{22},{23},{24},{25},{26},{27},{28},{29},{30}"
                                        , ToQuote(thisUser.ID)
                                        , ToQuote(thisUser.Username)
                                        , ToQuote(thisUser.Email)
                                        , ToQuote(thisUser.QQ)
                                        , ToQuote(thisUser.Mobile)
                                        , ToQuote(thisUser.IsSuper)
                                        , ToQuote(thisUser.Liked)
                                        , ToQuote(thisUser.Password)
                                        , ToQuote(thisUser.Province)
                                        , ToQuote(thisUser.City)
                                        , ToQuote(thisUser.ShopAddress)
                                        , ToQuote(thisUser.Availtime)
                                        , ToQuote(thisUser.BeautyAccount)
                                        , ToQuote(thisUser.BeautyPassword)
                                        , ToQuote(thisUser.Refer)
                                        , ToQuote(thisUser.Point)
                                        , ToQuote(thisUser.FreezePoint)
                                        , ToQuote(thisUser.Balance)
                                        , ToQuote(thisUser.Role)
                                        , ToQuote(thisUser.ZFB)
                                        , ToQuote(thisUser.Card)
                                        , ToQuote(thisUser.IsLogin)
                                        , ToQuote(thisUser.Ip)
                                        , ToQuote(thisUser.Lastlogintime)
                                        , ToQuote(thisUser.Bank)
                                        , ToQuote(thisUser.Rate)
                                        , thisUser.TimePoint.HasValue ? ToQuote(thisUser.TimePoint.Value) : "NULL"
                                        , thisUser.MaxPoint.HasValue ? ToQuote(thisUser.MaxPoint.Value) : "NULL"
                                        , ToQuote(CurrentUserName)
                                        , ToQuote(DateTime.Now)
                                        , ToQuote(thisUser.Statues)

                                    );
            try
            {
                int rowcount = SqlHelper.ExecuteNonQuery(ConnectStr, CommandType.Text, sql);

                if (rowcount == 1)
                {
                    return true;
                }
                else
                {
                    throw new Exception("SQL execution failed");
                }

            }
            catch (Exception ex)
            {
                throw new Exception("SQL execution failed", ex);
            }
        }

        public bool LoginActive(string username,string ip)
        {
            string sql = "update [users] set islogin = 1 ,ip='" + ip + "' ,lastlogintime = '" + DateTime.Now.ToString() + "' where username = '" + username + "'";

            try
            {
                int rowcount = SqlHelper.ExecuteNonQuery(ConnectStr, CommandType.Text, sql);

                if (rowcount >= 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception ex)
            {
                return false;
            }
        }


        /// <summary>
        /// Delete an existing User
        /// </summary>
        /// <param name="thisUser">User</param>
        /// <returns>bool</returns>
        public bool Delete(User thisUser)
        {
            string sql = string.Format("exec dbo.sp_Users_d {0} "
                                        , ToQuote(thisUser.ID)

                                      );

            try
            {
                int rowcount = SqlHelper.ExecuteNonQuery(ConnectStr, CommandType.Text, sql);

                if (rowcount >= 1)
                {
                    return true;
                }
                else
                {
                    throw new Exception("SQL execution failed");
                }

            }
            catch (Exception ex)
            {
                throw new Exception("SQL execution failed", ex);
            }
        }


        /// <summary>
        /// Check if a User already exists
        /// </summary>
        /// <param name="name">User Name</param>
        /// <returns>bool</returns>
        public bool Exists(string name)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get a User by id
        /// </summary>
        /// <param name="id">User id</param>
        /// <returns>User</returns>
        public User Get(Guid id)
        {
            string sql = string.Format("EXEC sp_Users_g {0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12}"
                                , ToQuote(id)
                                , "NULL"
                                , "NULL"
                                , "NULL"
                                , "NULL"
                                , "NULL"
                                , "NULL"
                                , "NULL"
                                , "NULL"
                                , "NULL"
                                , "NULL"
                                , "NULL"
                                , "NULL"
                           
                             );

            try
            {
                PaginationInfo paing = new PaginationInfo();
                IList<User> Users = GetUsers(sql, 0, null, out paing);
                if (Users.Count > 0)
                    return Users[0];
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception("SQL execution failed", ex);
            }
        }

        /// <summary>
        /// Get all Users
        /// </summary>
        /// <param name="activated">activated?</param>
        /// <param name="User">current User</param>
        /// <param name="sortKey">sort key</param>
        /// <returns></returns>
        public IList<User> Get(Guid? userid, string username, string mobile, string QQ, string email, bool? issuper, int? liked,
            int? role, string refer, bool? islogin, int? statues, int Page, string sortKey, out PaginationInfo paing)
        {
            string sql = string.Format("EXEC sp_Users_g {0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12}"
                             , "NULL"
                             , ToQuote(username)
                             , ToQuote(email)
                             , ToQuote(QQ)
                             , ToQuote(mobile)
                             , issuper.HasValue ? ToQuote(issuper.Value) : "NULL"
                             , liked.HasValue ? ToQuote(liked.Value) : "NULL"
                             , ToQuote(refer)
                             , ToQuote(role)
                             , islogin.HasValue ? ToQuote(islogin.Value) : "NULL"
                             , statues.HasValue ? ToQuote(statues.Value) : "NULL"
                             , ToQuote(Page)
                             , ToQuote(sortKey)
                          );

            try
            {

                IList<User> Users = GetUsers(sql, Page, null, out paing);
                return Users;
            }
            catch (Exception ex)
            {
                throw new Exception("SQL execution failed", ex);
            }
        }

        public int CheckconditionUser(int like, bool issuper, string username)
        {
            string sql = "select count(*) from [users] where username<>'" + username + "' and islogin = 1 and IsSuper = " + (issuper ? "1" : "0");
            if (like == 5000)
            {
                sql += " and liked<5000";
            }
            else if (like == 10000)
            {
                sql += " and liked>=5000 and liked<10000";
            }
            else if (like == 20000)
            {
                sql += " and liked>=10000 and liked<20000";
            }
            else if (like == 30000)
            {
                sql += " and liked>=20000 and liked<30000";
            }
            else if (like == 50000)
            {
                sql += " and liked>=30000 and liked<=50000";
            }
            else if (like == 50001)
            {
                sql += " and liked>50000";
            }
            object count = SqlHelper.ExecuteScalar(ConnectStr, CommandType.Text, sql);
            if ((int)count > 0)
                return 0;

            sql = "select count(*) from [users] where username<>'" + username + "' and IsSuper = " + (issuper ? "1" : "0");
            if (like == 5000)
            {
                sql += " and liked<5000";
            }
            else if (like == 10000)
            {
                sql += " and liked>=5000 and liked<10000";
            }
            else if (like == 20000)
            {
                sql += " and liked>=10000 and liked<20000";
            }
            else if (like == 30000)
            {
                sql += " and liked>=20000 and liked<30000";
            }
            else if (like == 50000)
            {
                sql += " and liked>=30000 and liked<=50000";
            }
            else if (like == 50001)
            {
                sql += " and liked>50000";
            }

            count = SqlHelper.ExecuteScalar(ConnectStr, CommandType.Text, sql);

            if ((int)count > 0)
                return 1;

            return 2;
        }

        public IList<User> GetByAdmin(Guid? userid, string username, string mobile, string QQ, string email, bool? issuper, int? liked,
             int? role, string refer, bool? islogin, int? statues, int Page, string sortKey, out PaginationInfo paing)
        {
            string sql = string.Format("EXEC sp_Users_g_admin {0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12}"
                             , "NULL"
                             , ToQuote(username)
                             , ToQuote(email)
                             , ToQuote(QQ)
                             , ToQuote(mobile)
                             , issuper.HasValue ? ToQuote(issuper.Value) : "NULL"
                             , liked.HasValue ? ToQuote(liked.Value) : "NULL"
                             , ToQuote(refer)
                             , ToQuote(role)
                             , islogin.HasValue ? ToQuote(islogin.Value) : "NULL"
                             , statues.HasValue ? ToQuote(statues.Value) : "NULL"
                             , ToQuote(Page)
                             , ToQuote(sortKey)
                          );

            try
            {

                IList<User> Users = GetUsers(sql, Page, null, out paing);
                return Users;
            }
            catch (Exception ex)
            {
                throw new Exception("SQL execution failed", ex);
            }
        }

        #endregion
    }
}
