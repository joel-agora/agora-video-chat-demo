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
    [SerializeField]
    private int remotePlayerID;

    [SerializeField]
    private string remoteInviteChannelName;

    // Start is called before the first frame update
    void Start()
    {
        photonView = GetComponent<PhotonView>();

        if(!photonView.isMine)
        {
            transform.GetChild(0).gameObject.SetActive(false);
        }

        inviteButton.interactable = false;
        joinButton.interactable = false;
    }

    [PunRPC]
    public void Test(string channelName)
    {
        print("IM THE TARGET");
        joinButton.interactable = true;
    }

    [PunRPC]
    public void AllTest()
    {
        print(gameObject.name + " ALL Test"); // this called on each client, but acts as if the call came from the specific client that called it
    }

    // this button press will always be local because the remote clients canvases are disabled
    public void OnInviteButtonPress()
    {
        if(remotePlayerID != -1)
        {
            photonView.RPC("Test", PhotonPlayer.Find(remotePlayerID), "testChannel");
        }
    }

    public void OnJoinButtonPress()
    {
        print("I'm going to join players channel: ");
    }

    // this scripts fire everywhere
    // each of these objects are now essentially in the scene, and you have to sort them out as such.
    private void OnTriggerEnter(Collider other)
    {
        if (!photonView.isMine)
        {
            return;
        }

        if (other.CompareTag("Player"))
        {
            print("I bumped into: " + other.name);   
            remotePlayerID = PhotonView.Get(other.gameObject).ownerId;
            inviteButton.interactable = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(!photonView.isMine)
        {
            return;
        }

        if(other.CompareTag("Player"))
        {
            remotePlayerID = -1;    

            inviteButton.interactable = false;
        }
    }
}
