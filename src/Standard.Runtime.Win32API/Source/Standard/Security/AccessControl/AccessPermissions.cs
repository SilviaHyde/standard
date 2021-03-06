using System;

namespace Standard.Security.AccessControl
{
   /// <summary>A set of bit flags that describe the permissions for the shared resource's on servers running with share-level security.</summary>
   /// <remarks>Note that Windows does not support share-level security. This member is ignored on a server running user-level security.</remarks>
   [Flags]
   public enum AccessPermissions
   {
      /// <summary>No permissions.</summary>
      None = 0,

      /// <summary>
      /// <para>Permission to read data from a resource and, by default, to execute the resource.</para>
      /// </summary>
      /// <devdoc>ACCESS_READ</devdoc>
      Read = 1,

      /// <summary>
      /// <para>Permission to write data to the resource.</para>
      /// </summary>
      /// <devdoc>ACCESS_WRITE</devdoc>
      Write = 2,

      /// <summary>
      /// <para>Permission to create an instance of the resource (such as a file); data can be written to the resource as the resource is created.</para>
      /// </summary>
      /// <devdoc>ACCESS_CREATE</devdoc>
      Create = 4,

      /// <summary>
      /// <para>Permission to execute the resource.</para>
      /// </summary>
      /// <devdoc>ACCESS_EXEC</devdoc>
      Execute = 8,

      /// <summary>
      /// <para>Permission to delete the resource.</para>
      /// </summary>
      /// <devdoc>ACCESS_DELETE</devdoc>
      Delete = 16,

      /// <summary>
      /// <para>Permission to modify the resource's attributes, such as the date and time when a file was last modified.</para>
      /// </summary>
      /// <devdoc>ACCESS_ATRIB</devdoc>
      Attributes = 32,

      /// <summary>
      /// <para>Permission to modify the permissions (read, write, create, execute, and delete) assigned to a resource for a user or application.</para>
      /// </summary>
      /// <devdoc>ACCESS_PERM</devdoc>
      Permissions = 64,

      /// <summary>
      /// <para>Permission to read, write, create, execute, and delete resources, and to modify their attributes and permissions.</para>
      /// </summary>
      /// <devdoc>ACCESS_ALL</devdoc>
      All = 32768
   }
}
