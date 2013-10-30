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

    public class PriceService : BaseService, IPrice
    {
        #region private function

        /// <summary>
        /// Don't repeat yourself! This private function is for shared by all query public functions
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="page"></param>
        /// <param name="pg"></param>
        /// <returns></returns>
        IList<BeautyPrice> GetBeautyPrices(string sql, int page, string sortKey, out PaginationInfo paging)
        {
            using (DataSet ds = SqlHelper.ExecuteDataset(ConnectStr, CommandType.Text, sql))
            {
                if (ds == null || ds.Tables.Count != 2)
                {
                    throw new Exception("SQL execution failed");
                }
                else
                {
                    List<BeautyPrice> Comments = new List<BeautyPrice>();

                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        BeautyPrice Comment = new BeautyPrice()
                        {
                            ID = new Guid(dr["id"].ToString()),
                            Price = ParseDecimal(dr["Price"].ToString()),
                            Pricename = ParseStr(dr["Pricename"]),
                            PromotionPrice = ParseNDecimal(dr["PromotionPrice"].ToString()),
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
        /// Create a new BeautyPrice
        /// </summary>
        /// <param name="newBeautyPrice">new BeautyPrice</param>
        /// <returns>new BeautyPrice id</returns>
        public Guid Create(BeautyPrice newBeautyPrice)
        {
            string sql = string.Format("EXEC sp_Price_c {0},{1},{2},{3},{4},{5},{6},{7},{8}"
                                         , ToQuote(newBeautyPrice.ID)
                                         , ToQuote(newBeautyPrice.Pricename)
                                         , ToQuote(newBeautyPrice.Price)
                                         , ToQuote(newBeautyPrice.PromotionPrice)
                                         , ToQuote(CurrentUserName)
                                         , ToQuote(DateTime.Now)
                                         , ToQuote(CurrentUserName)
                                         , ToQuote(DateTime.Now)
                                         , ToQuote(newBeautyPrice.Statues)

                                     );

            try
            {
                int rowcount = SqlHelper.ExecuteNonQuery(ConnectStr, CommandType.Text, sql);

                if (rowcount == 1)
                {
                    return newBeautyPrice.ID;
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
        /// Update an existing BeautyPrice
        /// </summary>
        /// <param name="thisBeautyPrice">BeautyPrice</param>
        /// <returns>bool</returns>
        public bool Update(BeautyPrice thisBeautyPrice)
        {
            string sql = string.Format("EXEC sp_Price_u {0},{1},{2},{3},{4},{5},{6}"
                                         , ToQuote(thisBeautyPrice.ID)
                                         , ToQuote(thisBeautyPrice.Pricename)
                                         , ToQuote(thisBeautyPrice.Price)
                                         , ToQuote(thisBeautyPrice.PromotionPrice)
                                         , ToQuote(CurrentUserName)
                                         , ToQuote(DateTime.Now)
         
                                         , ToQuote(thisBeautyPrice.Statues)
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

        /// <summary>
        /// Delete an existing BeautyPrice
        /// </summary>
        /// <param name="thisBeautyPrice">BeautyPrice</param>
        /// <returns>bool</returns>
        public bool Delete(BeautyPrice thisBeautyPrice)
        {
            string sql = string.Format("exec dbo.sp_Price_d {0} "
                                        , ToQuote(thisBeautyPrice.ID)

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
        /// Check if a BeautyPrice already exists
        /// </summary>
        /// <param name="name">BeautyPrice Name</param>
        /// <returns>bool</returns>
        public bool Exists(string name)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get a BeautyPrice by id
        /// </summary>
        /// <param name="id">BeautyPrice id</param>
        /// <returns>BeautyPrice</returns>
        public BeautyPrice Get(Guid id)
        {
            string sql = string.Format("EXEC sp_Price_g {0},{1},{2},{3},{4}"
                                , ToQuote(id)
                                , "NULL"
                                , "NULL"
                                , "NULL"
                                , "NULL"
                             );

            try
            {
                PaginationInfo paing = new PaginationInfo();
                IList<BeautyPrice> BeautyPrices = GetBeautyPrices(sql, 0, null, out paing);
                if (BeautyPrices.Count > 0)
                    return BeautyPrices[0];
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception("SQL execution failed", ex);
            }
        }

        /// <summary>
        /// Get all BeautyPrices
        /// </summary>
        /// <param name="activated">activated?</param>
        /// <param name="BeautyPrice">current BeautyPrice</param>
        /// <param name="sortKey">sort key</param>
        /// <returns></returns>
        public IList<BeautyPrice> Get(Guid? id, string pricename, int? statues, int Page, string sortKey, out PaginationInfo paing)
        {
            string sql = string.Format("EXEC sp_Price_g {0}, {1},{2},{3},{4}"
                             , "NULL"
                             , ToQuote(pricename)
                             , statues.HasValue ? ToQuote(statues.Value) : "NULL"
                             , ToQuote(Page)
                             , ToQuote(sortKey)
                          );

            try
            {

                IList<BeautyPrice> BeautyPrices = GetBeautyPrices(sql, Page, null, out paing);
                return BeautyPrices;
            }
            catch (Exception ex)
            {
                throw new Exception("SQL execution failed", ex);
            }
        }

        #endregion
    }
}
