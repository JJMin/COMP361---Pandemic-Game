/**/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using MySql.Data.MySqlClient;
using System.Data;


public class Database : MonoBehaviour
{

	IDbConnection dbcon;

	IDbCommand cmd;

	public static Database instance = null;

	private string connectionString = "Server=142.157.30.161; Port=3306; Database=pandemic; User ID = newuser; Password= password; Pooling=false"; 

	// Setup connection to the database
	void Awake()
	{

		instance = this;
		openConnection();
	}

	private void openConnection()
	{
		dbcon = new MySqlConnection(connectionString);
		dbcon.Open();
		cmd = dbcon.CreateCommand();
	}

	public void closeConnection()
	{

		dbcon.Close();
		dbcon = null;
		cmd.Dispose();
		cmd = null;

	}

	//checks if username exists and the corresponding password is correct.
	// Returns -1 if user does not exist, 0 if password is incorrect, 1 if both are correct. 
	public int validateUserDetails(string username, string password)
	{

		string sql =
			"SELECT username, password " +
			"FROM users" +
			" WHERE username = " + "'" + username + "'";

		cmd.CommandText = sql;

		IDataReader reader = cmd.ExecuteReader();

		while (reader.Read())
		{


			Debug.Log("In reader.read()");
			string user = (string)reader["username"];
			string pwd = (string)reader["password"];

			//Debug.Log("Database found user: " + user +" and password: "+pwd);

			if (user == username)
			{

				if (pwd == password)
				{
					reader.Close();
					return 1;
				}
				else
				{
					reader.Close();
					return 0;
				}
			}
		}

		//Debug.Log ("Out of reader.read() ");

		reader.Close();
		return -1;
	}

	//Add new user to database. Returns 1 if process was successful, -1 if user already exist and 0 if there was a problem adding user
	public int addUser(string username, string password)
	{

		if (validateUserDetails(username, password) != -1)
		{
			return -1;
		}

		string sql =
			"INSERT INTO users (username, password) VALUES (" + "'" + username + "'" + "," + "'" + password + "'" + ")";

		cmd.CommandText = sql;
		int i = cmd.ExecuteNonQuery();

		if (i > 0)
		{       //check if it was successful
			return 1;
		}

		return 0;

	}

	//saves this user's game on the database 
	public Boolean saveGameOnDatabase(string username, MyGames game)
	{
		openConnection();

		string sql = "UPDATE users SET games = LOAD_FILE('"+ game + "') WHERE username = " + "'" + username + "'"; //the game shouuld be a path to games folder
		cmd.CommandText = sql;
		cmd.CommandText = sql;
		int i = cmd.ExecuteNonQuery();

		if (i > 0)
		{       //check if it was successful
			return true;
		}

		return false;
	}
}
/**/
