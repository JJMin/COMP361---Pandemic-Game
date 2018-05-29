using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;


public class RegLoginUIcheck : MonoBehaviour
{
	public UIInput userNameLogin;
	public UIInput pwdLogin;

	public UIInput userNameReg;
	public UIInput pwdReg;
	public UIInput cfmpwdReg;
	public GameObject noticePrefab;

	//当登录时用户输入完毕点击登录时 先本地检测用户输入
	public bool checkLogin()
	{
		if (userNameLogin.value == "" || pwdLogin.value == "")
		{//用户名或密码为空

			generateNotice("Username or Password is empty!");
			return false;
		}

		/**/
		int result = Database.instance.validateUserDetails(userNameLogin.value, pwdLogin.value);
		if (result == -1)
		{
			generateNotice("Username does not exist!");
			return false;
		}
		else if (result == 0)
		{
			generateNotice("Wrong Password!");
			return false;
		}/**/

		playerDetails.Instance.username = userNameLogin.value;          // assign username here
																		//Database.instance.closeConnection();

		return true;
	}
	public bool checkRegister()
	{
		string username = userNameReg.value;
		string password1 = pwdReg.value;
		string password2 = cfmpwdReg.value;


		//username and password not empty
		if (username == "" || password1 == "" || password2 == "")
		{
			generateNotice("Username or Password is empty!");
			return false;
		}

		//password match
		if (password1 != password2)
		{
			generateNotice("Password do not match!");
			return false;
		}


		Regex num = new Regex("^[0-9]");
		if (num.IsMatch(username))
		{
			generateNotice("Username can not start with a number or have no alpha characters!");
			return false;
		}

		Regex alphaNum = new Regex("^[a-zA-Z0-9]*$");
		//alphabet or alpha-numeric
		if (!alphaNum.IsMatch(username))
		{
			generateNotice("Username can contain only alpha or alpha-numeric characters!");
			return false;
		}

		//password length
		if (pwdReg.value.Length < 6)
		{
			generateNotice("Password is too short! \n Must be at least 6 characters.");
			return false;
		}

		/**/
		int result = Database.instance.addUser(username, password1);
		if (result == -1)
		{
			generateNotice("Username already exists!");
			return false;
		}
		else if (result == 0)
		{
			generateNotice("Registration failed, please try again");
			return false;
		}
		else if (result == 1)
		{
			Debug.Log("Registration successful");
		}/**/

		username = "";
		password1 = "";
		password2 = "";


		return true;
	}

	GameObject generateNotice(string content)
	{
		Debug.Log("in generateNotice");
		GameObject notice = NGUITools.AddChild(this.gameObject, noticePrefab);
		notice.transform.Find("noticeText").GetComponent<UILabel>().text = content;
		return notice;

	}
}
