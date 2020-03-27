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

    public int playerToInviteID;

    // Start is called before the first frame update
    void Start()
    {
        photonView = GetComponent<PhotonView>();

        if(!photonView.isMine)
        {
            //transform.GetChild(0).gameObject.SetActive(false);
        }
    }

    [PunRPC]
    public void Test()
    {
        print(gameObject.name + " HAVE BEEN TESTED!");
    }

    [PunRPC]
    public void AllTest()
    {
        print(gameObject.name + " ALL Test");
    }

    public void OnInviteButtonPress()
    {
        print(gameObject.name + "pressed invite button");
        photonView.RPC("Test", PhotonPlayer.Find(playerToInviteID));

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

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            playerToInviteID = other.GetComponent<PhotonView>().viewID;

            inviteButton.interactable = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            playerToInviteID = -1;

            inviteButton.interactable = false;
        }
    }
}
