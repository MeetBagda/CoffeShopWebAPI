using CoffeeShopWebAPI.Models;
using System.Data.SqlClient;
using System.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;

namespace CoffeeShopWebAPI.Data
{
    public class BillRepository
    {
        private readonly string _connectionString;

        public BillRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ConnectionString");
        }

        #region SelectAll
        public IEnumerable<BillModel> SelectAll()
        {
            var bills = new List<BillModel>();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("spBill_SelectAll", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    bills.Add(new BillModel
                    {
                        BillID = Convert.ToInt32(reader["BillID"]),
                        BillNumber = reader["BillNumber"].ToString(),
                        BillDate = Convert.ToDateTime(reader["BillDate"]),
                        CustomerID = Convert.ToInt32(reader["CustomerID"]),
                        OrderID = Convert.ToInt32(reader["OrderID"]),
                        TotalAmount = Convert.ToDecimal(reader["TotalAmount"]),
                        Discount = reader["Discount"] != DBNull.Value ? Convert.ToDecimal(reader["Discount"]) : (decimal?)null,
                        NetAmount = Convert.ToDecimal(reader["NetAmount"]),
                        UserID = Convert.ToInt32(reader["UserID"])
                    });
                }
                return bills;
            }
        }
        #endregion

        #region SelectById
        public BillModel SelectById(int billID)
        {
            BillModel bill = null;
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("spBill_SelectById", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@BillID", billID);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    bill = new BillModel
                    {
                        BillID = Convert.ToInt32(reader["BillID"]),
                        BillNumber = reader["BillNumber"].ToString(),
                        BillDate = Convert.ToDateTime(reader["BillDate"]),
                        CustomerID = Convert.ToInt32(reader["CustomerID"]),
                        OrderID = Convert.ToInt32(reader["OrderID"]),
                        TotalAmount = Convert.ToDecimal(reader["TotalAmount"]),
                        Discount = reader["Discount"] != DBNull.Value ? Convert.ToDecimal(reader["Discount"]) : (decimal?)null,
                        NetAmount = Convert.ToDecimal(reader["NetAmount"]),
                        UserID = Convert.ToInt32(reader["UserID"])
                    };
                }
                return bill;
            }
        }
        #endregion

        #region Delete
        public bool Delete(int billID)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("spBill_Delete", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@BillID", billID);
                conn.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
                return rowsAffected > 0;
            }
        }
        #endregion

        #region Insert
        public void Insert(BillModel bill)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("spBill_Insert", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@BillNumber", bill.BillNumber);
                cmd.Parameters.AddWithValue("@BillDate", bill.BillDate);
                cmd.Parameters.AddWithValue("@CustomerID", bill.CustomerID);
                cmd.Parameters.AddWithValue("@OrderID", bill.OrderID);
                cmd.Parameters.AddWithValue("@TotalAmount", bill.TotalAmount);
                cmd.Parameters.AddWithValue("@Discount", bill.Discount ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@NetAmount", bill.NetAmount);
                cmd.Parameters.AddWithValue("@UserID", bill.UserID);

                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    throw new Exception("An error occurred while inserting the bill: " + ex.Message, ex);
                }
            }
        }
        #endregion

        #region Update
        public void Update(BillModel bill)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("spBill_Update", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@BillID", bill.BillID);
                cmd.Parameters.AddWithValue("@BillNumber", bill.BillNumber);
                cmd.Parameters.AddWithValue("@BillDate", bill.BillDate);
                cmd.Parameters.AddWithValue("@CustomerID", bill.CustomerID);
                cmd.Parameters.AddWithValue("@OrderID", bill.OrderID);
                cmd.Parameters.AddWithValue("@TotalAmount", bill.TotalAmount);
                cmd.Parameters.AddWithValue("@Discount", bill.Discount ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@NetAmount", bill.NetAmount);
                cmd.Parameters.AddWithValue("@UserID", bill.UserID);

                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    throw new Exception("An error occurred while updating the bill: " + ex.Message, ex);
                }
            }
        }
        #endregion

        //#region SelectDropdown
        //public IEnumerable<BillDropDownModel> SelectDropdown()
        //{
        //    var billsDropdown = new List<BillDropDownModel>();
        //    using (SqlConnection conn = new SqlConnection(_connectionString))
        //    {
        //        SqlCommand cmd = new SqlCommand("spBill_SelectDropdown", conn)
        //        {
        //            CommandType = CommandType.StoredProcedure
        //        };

        //        conn.Open();

        //        using (var reader = cmd.ExecuteReader())
        //        {
        //            while (reader.Read())
        //            {
        //                billsDropdown.Add(new BillDropDownModel
        //                {
        //                    BillID = Convert.ToInt32(reader["BillID"]),
        //                    BillNumber = reader["BillNumber"].ToString()
        //                });
        //            }

        //            return billsDropdown;
        //        }
        //    }
        //}
        //#endregion
    }
}