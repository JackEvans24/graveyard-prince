using UnityEngine;

public static class Collider2DExtensions
{
    public static void AssignIfTagMatches<T>(this Collider2D collision, ref T field, string tag)
    {
        if (collision.CompareTag(tag))
        {
            var component = collision.GetComponent<T>();
            if (component != null)
                field = component;
        }
    }

    public static void UnassignIfTagMatches<T>(this Collider2D collision, ref T field, string tag) where T : class
    {
        if (collision.CompareTag(tag))
            field = null;
    }
}
