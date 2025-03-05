using CoffeeShopWebAPI.Models;
using System.Data.SqlClient;
using System.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;

namespace CoffeeShopWebAPI.Data
{
    public class StateRepository
    {
        private readonly string _connectionString;

        public StateRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ConnectionString");
        }

        #region SelectAll
        public List<StateModel> SelectAll()
        {
            var states = new List<StateModel>();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("PR_LOC_State_SelectAll", conn)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    states.Add(new StateModel
                    {

                        StateID = Convert.ToInt32(reader["StateID"]),
                        StateName = reader["StateName"].ToString(),
                        StateCode = reader["StateCode"].ToString(),

                    }
                    );
                }
                return states;
            }
        }
        #endregion

        #region SelectByPK
        public StateModel SelectByPK(int stateID)
        {
            StateModel state = null;
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("PR_LOC_State_SelectByPK", conn)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@StateID", stateID);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    state = new StateModel
                    {
                        StateID = Convert.ToInt32(reader["StateID"]),
                        CountryID = Convert.ToInt32(reader["CountryID"]),
                        StateName = reader["StateName"].ToString(),
                        StateCode = reader["StateCode"].ToString(),
                       
                    };
                }
                return state;
            }
        }
        #endregion


        #region Delete
        public bool Delete(int stateID)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("PR_LOC_State_Delete", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@StateID", stateID);
                conn.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
                return rowsAffected > 0; // Return true if a row was deleted
            }
        }
        #endregion

        #region Insert
        public void Insert(StateModel state)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("PR_LOC_State_Insert", conn)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };

               
                cmd.Parameters.AddWithValue("@StateName", state.StateName);
                cmd.Parameters.AddWithValue("@StateCode", state.StateCode);
                cmd.Parameters.AddWithValue("@CountryID", state.CountryID);

                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery(); 
                }
                catch (SqlException ex)
                {
                    
                    throw new Exception("An error occurred while inserting the state: " + ex.Message, ex);
                }
            }
        }
        #endregion

        #region Update
        public void Update(StateModel state)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("PR_LOC_State_Update", conn)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };

                
                cmd.Parameters.AddWithValue("@StateID", state.StateID);
                cmd.Parameters.AddWithValue("@StateName", state.StateName);
                cmd.Parameters.AddWithValue("@StateCode", state.StateCode);
                cmd.Parameters.AddWithValue("@CountryID", state.CountryID);

                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery(); 
                }
                catch (SqlException ex)
                {
                   
                    throw new Exception("An error occurred while updating the state: " + ex.Message, ex);
                }
            }
        }
        #endregion
    }
}
