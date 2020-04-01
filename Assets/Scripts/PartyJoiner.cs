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

    private void OnEnable()
    {
        
        
    }

    private void OnDisable()
    {
        
    }

    [PunRPC]
    public void InvitePlayerToPartyChannel(string channelName)
    {
        remoteInviteChannelName = channelName;
        joinButton.interactable = true;
        print("I've been invited to join channel: " + remoteInviteChannelName);

        JoinButtonTest();
    }

    [PunRPC]
    void JoinButtonTest()
    {

        if(photonView.isMine)
        {
            print(gameObject.name + " testing for join button");
            joinButton.interactable = true;
            print(joinButton.interactable);

            transform.GetChild(0).transform.GetChild(1).GetComponent<Button>().interactable = true;
            print(joinButton.interactable);
        }
    }

    // this button press will always be local because the remote clients canvases are disabled
    public void OnInviteButtonPress()
    {
        if(remotePlayerID != -1)
        {
            photonView.RPC("InvitePlayerToPartyChannel", PhotonPlayer.Find(remotePlayerID), GetComponent<AgoraVideoChat>().GetRemoteChannel());
        }
    }

    public void OnJoinButtonPress()
    {
        print("I'm going to join players channel: " + remoteInviteChannelName);
        GetComponent<AgoraVideoChat>().JoinRemoteChannel(remoteInviteChannelName);
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
