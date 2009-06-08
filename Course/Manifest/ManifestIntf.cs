using System;

namespace FireFly.CourseEditor.Course.Manifest
{
    using System.Collections.Generic;

    ///<summary>
    ///  Generic list of manifest nodes
    ///</summary>
    ///<typeparam name="T"></typeparam>
    public interface IManifestNodeList<T> : IList<T>, IManifestNode
    {
    }

    /// <summary>
    /// This interface developed for make GUI support easier for different type of manifest types
    /// </summary>
    public interface ITitled
    {
        /// <summary>
        /// Gets or sets title of object
        /// </summary>
        [CanBeNull]
        string Title { get; set; }

        ///<summary>
        /// Raises when title changed
        ///</summary>
        event Action TitleChanged;
    }

    /// <summary>
    /// Represents any node it manifest
    /// </summary>
    public interface IManifestNode
    {
        /// <summary>
        /// Parent node for current
        /// </summary>
        [NotNull]
        IManifestNode Parent { get; set; }

        /// <summary>
        /// Unique identifier for node. Can be used as key for search node in a tree
        /// </summary>
        [NotNull]
        string UID { get; }

        /// <summary>
        /// Resolves Parent-Child relationship for this node and all children.
        /// </summary>
        void ResolveTree([NotNull]IManifestNode parent);
    }

    /// <summary>
    /// Represents any class of manifest that can contain other. Developed for make GUI support easier
    /// </summary>
    public interface IContainer : IManifestNode
    {
        /// <summary>
        /// Removed specified child of item. Implementation should throw exception if such child doesn't exists.
        /// Implementation SHOULDN'T search sub-nodes recursive.
        /// </summary>
        /// <param name="child"></param>
        void RemoveChild([NotNull]IManifestNode child);
    }

    /// <summary>
    /// Represents container of SCORM items. <see cref="OrganizationType"/> and <see cref="ItemType"/> are support it.
    /// </summary>
    public interface IItemContainer : IContainer
    {
        /// <summary>
        /// SCORM items contains in container
        /// </summary>
        ManifestNodeList<ItemType> SubItems { get; set; }
    }

    /// <summary>
    /// Represents a collection of SCORM organizations
    /// </summary>
    public interface IOrganizationContainer : IContainer
    {
        /// <summary>
        /// SCORM organizations contains in container
        /// </summary>
        ManifestNodeList<OrganizationType> Organizations { get; }
    }

    /// <summary>
    /// Represents a collection of SCORM resources.
    /// </summary>
    public interface IResourceContainer : IContainer
    {
        /// <summary>
        /// SCORM resources
        /// </summary>
        ManifestNodeList<ResourceType> Resources { get; }

        /// <summary>
        /// Provides quick way to get resource by this name. Implementation should return null if such resource doesn't exist.
        /// </summary>
        /// <param name="resourceName">Name of needed resource</param>
        /// <returns>Resource object if it found and null otherwise</returns>
        ResourceType this[[NotNull]string resourceName] { get; }
    }
}