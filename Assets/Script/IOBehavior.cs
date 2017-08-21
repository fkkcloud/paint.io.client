using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;

public class IOBehavior : MonoBehaviour {

	private SocketIOComponent _socketIO;
	public SocketIOComponent SocketIOComp
	{
		get
		{
			if (_socketIO == null)
				_socketIO = FindObjectOfType<SocketIOComponent> ();

			return _socketIO;
		}
	}

	private GameState _gameState;
	public GameState GlobalGameState
	{
		get
		{
			if (_gameState == null)
				_gameState = FindObjectOfType<GameState> ();

			return _gameState;
		}
	}

}
