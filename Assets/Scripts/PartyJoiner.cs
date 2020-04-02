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
    private int remoteButtonID;

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
    public void InvitePlayerToPartyChannel(string channelName)
    {
        remoteInviteChannelName = channelName;
        joinButton.interactable = true;
        print("I've been invited to join channel: " + remoteInviteChannelName);
    }

    // this button press will always be local because the remote clients canvases are disabled
    public void OnInviteButtonPress()
    {
        if(remotePlayerID != -1)
        {
            byte InviteEvent = 1;
            object content = null;
            int[] playerList = { PhotonPlayer.Find(remotePlayerID).ID };

            RaiseEventOptions raiseEventOptions = new RaiseEventOptions { TargetActors = playerList };

            //PhotonNetwork.RaiseEvent(InviteEvent, content, true, raiseEventOptions);

            photonView.RPC("InvitePlayerToPartyChannel", PhotonPlayer.Find(remotePlayerID), GetComponent<AgoraVideoChat>().GetRemoteChannel());
            photonView.RPC("ButtonScript", PhotonPlayer.Find(remoteButtonID));
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
            //print("I bumped into: " + other.name);   
            remotePlayerID = PhotonView.Get(other.gameObject).ownerId;
            remoteButtonID = PhotonView.Get(other.transform.GetChild(0).GetChild(1)).ownerId;

            print("button view PhotonViewID: " + remoteButtonID);

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
