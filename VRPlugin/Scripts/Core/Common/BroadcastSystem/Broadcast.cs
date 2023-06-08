using UnityEngine;

public class Broadcast<T> : Broadcast
{

    private BroadcastSender<T> _sender;
    private T _target;



    public T Target => _target;

    public Broadcast(Vector3 position, BroadcastSender<T> sender, T target)
    {
        _position = position;
        _sender = sender;
        _target = target;
    }


    public void ReleaseBroadcast()
    {
        _sender.ReleaseBroadcast();
    }
}

public class Broadcast
{
    protected Vector3 _position;
    public Vector3 Position => _position;
    public void UpdatePosition(Vector3 position)
    {
        _position = position;
    }

}