﻿namespace Standard.IO.Compression.LZ4Encoding
{
    /// <summary>
    /// Action performed by encoder using internal `FlushAndEncode` method.
    /// </summary>
    internal enum EncoderAction
	{
		/// <summary>
        /// Nothing has happened, most likely loading 0 bytes.
        /// </summary>
		None,

		/// <summary>
        /// Some bytes has been loaded into encoder.
        /// </summary>
		Loaded,

		/// <summary>
        /// Compression was not possible so bytes has been copied.
        /// </summary>
		Copied,

		/// <summary>
        /// Compression succeeded.
        /// </summary>
		Encoded,
	}
}
