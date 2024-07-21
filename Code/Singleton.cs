using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T: Singleton<T>
{
	private static T cachedInstance;
	public static T Instance 
	{
		get
		{
			cachedInstance ??= FindObjectOfType<T>();
			
			if(cachedInstance == null)
			{
				throw new System.NullReferenceException($"Cannot find singleton of type {typeof(T)}");
			}
			
			return cachedInstance;	
		}
	}
}
