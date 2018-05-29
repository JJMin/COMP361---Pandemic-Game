using UnityEngine;
using UnityEngine.UI;

public class PlayerListing : MonoBehaviour {

	public PhotonPlayer PhotonPlayer { get; private set; }

	[SerializeField]
	private Text _playerName;

	public Text PlayerName{
		get { return _playerName; }
	}

	//assigns user's given nickame to text
	public void ApplyPhotonPlayer( PhotonPlayer photonPlayer){

		PhotonPlayer = photonPlayer;			//this assignment makes sure the photonPlayer name is attached to the player on the network
		PlayerName.text = photonPlayer.NickName;
	}


}
