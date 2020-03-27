using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PartyJoiner : MonoBehaviour
{

    private PhotonView photonView;
    // when the invite is made, send them your channelName
    // when two players separate, nullify/disable everything
    // when the other player presses join, have them join the special channel

    //[SerializeField]
    //private Canvas playerCanvas;

    [SerializeField]
    private Button inviteButton;
    [SerializeField]
    private Button joinButton;

    public string remotePlayerName;

    // Start is called before the first frame update
    void Start()
    {
        photonView = GetComponent<PhotonView>();

        if(!photonView.isMine)
        {
            transform.GetChild(0).gameObject.SetActive(false);
        }
    }

    [PunRPC]
    public void Test(int remoteID)
    {
        //print(photonView.name + " HAVE BEEN TESTED!");// my local client told Remote to say MY name, because in this function I'm printing MY name
        //print("photon view name: " + photonView.name);

        print(PhotonPlayer.Find(remoteID).NickName);
    }

    [PunRPC]
    public void AllTest()
    {
        print(gameObject.name + " ALL Test"); // this called on each client, but acts as if the call came from the specific client that called it
    }

    // this button press will always be local because the remote clients canvases are disabled
    public void OnInviteButtonPress()
    {
        print(gameObject.name + "pressed invite button");
        //photonView.RPC("Test", PhotonPlayer.Find(playerToInviteID)); // this is passed to other player

        photonView.RPC("AllTest", PhotonTargets.All);
    }

    public void OnJoinButtonPress()
    {
        print("join button");

        if(photonView.isMine)
        {
            print("MY jion");
        }
    }

    // this scripts fire everywhere
    // each of these objects are now essentially in the scene, and you have to sort them out as such.
    private void OnTriggerEnter(Collider other)
    {
        if (!photonView.isMine)
            return;

        

        if(other.CompareTag("Player"))
        {
            //print(other.name);
            int id;
            id = PhotonView.Get(other.gameObject).ownerId;
            photonView.RPC("Test", PhotonPlayer.Find(id), id);
            //print("test 1");
            
            id = PhotonView.Get(other.gameObject).viewID;
            photonView.RPC("Test", PhotonPlayer.Find(id), id);
            //print("test 2");

            

            //remotePlayerName = PhotonView.Get(other.gameObject).GetInstanceID().ToString();
            //print("PhotonView.Get.GetInstanceID: " + remotePlayerName);

            //remotePlayerName = PhotonView.Get(other.gameObject).name.ToString();
            //print("PhotonView.Get.viewId: " + remotePlayerName);

            //playerToInviteID = PhotonPlayer.;

            //inviteButton.interactable = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {


        if(other.CompareTag("Player"))
        {
            
            //playerToInviteID = -1;

            //inviteButton.interactable = false;
        }
    }
}
