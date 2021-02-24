//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

using System;
using System.Text.RegularExpressions;

namespace MS.GTA.CommonDataService.Common.Internal
{
    /// <summary>
    /// To help with representing and performing operations on semantic versions (<see cref="http://semver.org/spec/v2.0.0.html"/>).
    /// Comparisons are performed using the semantic version specification (i.e. build version is ignored).
    /// The implementation of the <see cref="IEquatable{T}"/> and <see cref="IComparable{T}"/> interfaces are done
    /// along C# guidelines and will check for object equality and comparability by also comparing the build version.
    /// </summary>
    /// <remarks>
    /// These APIs support the SDK infrastructure and are not intended to be used
    /// directly from your code. The APIs may change or be removed in future releases.
    /// </remarks>
    /// <example>Major.Minor.Patch-Prerelease+Build</example>
    public sealed class SemanticVersion : IEquatable<SemanticVersion>, IComparable<SemanticVersion>
    {
        // Valid prerelease and build regex
        private const string PBRegexString = @"[A-Za-z0-9\-\.]+";

        // Full SemVer regex
        private static readonly Regex SemVerRegex = new Regex(
            $@"^(?<major>\d+)\.(?<minor>\d+)\.(?<patch>\d+)(-(?<prerelease>{PBRegexString}))?(\+(?<build>{PBRegexString}))?$",
            RegexOptions.Singleline);

        // Just match a prerease or build version regex
        private static readonly Regex PBRegex = new Regex(
            $"^({PBRegexString})$",
            RegexOptions.Singleline);

        /// <summary>
        /// Construct a semantic version with a given major, minor, and patch version number.
        /// All must be nonzero integers according to the semantic version spec.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">Throws when a version number is less than 0.</exception>
        public SemanticVersion(int major, int minor, int patch)
            : this(major, minor, patch, null, null)
        {
        }

        /// <summary>
        /// Construct a semantic version with a given major, minor, patch, prerelease, and build version.
        /// Major, minor, and patch must be nonzero integers with prerelease and build being alphanumeric strings according to the semantic version spec.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">Throws when a numeric version number is less than 0.</exception>
        public SemanticVersion(int major, int minor, int patch, string prereleaseTag, string buildMetadata)
        {
            SetMajorMinorPatch(major, minor, patch);

            CheckValidPrereleaseOrBuildString(prereleaseTag, nameof(prereleaseTag));
            CheckValidPrereleaseOrBuildString(buildMetadata, nameof(buildMetadata));
            PrereleaseTag = prereleaseTag;
            BuildMetadata = buildMetadata;
        }

        /// <summary>
        /// Wrapper to set and validate all three.
        /// </summary>
        private void SetMajorMinorPatch(int major, int minor, int patch)
        {
            Contract.CheckRange(major >= 0, nameof(major), $"{nameof(major)} cannot be <0 (is: {major})");
            Contract.CheckRange(minor >= 0, nameof(minor), $"{nameof(minor)} cannot be <0 (is: {minor})");
            Contract.CheckRange(patch >= 0, nameof(patch), $"{nameof(patch)} cannot be <0 (is: {patch})");

            Major = major;
            Minor = minor;
            Patch = patch;
        }

        public int Major { get; private set; }
        public int Minor { get; private set; }
        public int Patch { get; private set; }
        public string PrereleaseTag { get; private set; }
        public string BuildMetadata { get; private set; }

        /// <summary>
        /// Increase the major version number by 1 and reset the following.
        /// </summary>
        public SemanticVersion IncrementMajor()
        {
            return new SemanticVersion(Major + 1, 0, 0);
        }

        /// <summary>
        /// Increase the minor version number by 1 and reset the following.
        /// </summary>
        public SemanticVersion IncrementMinor()
        {
            return new SemanticVersion(Major, Minor + 1, 0);
        }

        /// <summary>
        /// Increase the patch version number by 1 and reset the following.
        /// </summary>
        public SemanticVersion IncrementPatch()
        {
            return new SemanticVersion(Major, Minor, Patch + 1);
        }

        private static void CheckValidPrereleaseOrBuildString(string version, string paramName)
        {
            if (ReferenceEquals(null, version))
            {
                return;
            }

            Contract.CheckNonWhitespace(version, paramName, $"The {paramName} string cannot be null or whitespace.");

            Match match = PBRegex.Match(version);
            if (!match.Success)
            {
                throw new ArgumentException($"The {paramName} string \"{version}\" is not valid semver.", paramName);
            }
        }

        /// <summary>
        /// Set the prerelease tag.
        /// </summary>
        /// <example>UpdatePrereleaseTag("alpha")</example>
        /// <example>UpdatePrereleaseTag("alpha.1")</example>
        /// <example>UpdatePrereleaseTag("0.3.7")</example>
        /// <example>UpdatePrereleaseTag("x.7.z.92")</example>
        /// <exception cref="ArgumentNullException">Throws when the parameter is null or whitespace.</exception>
        /// <exception cref="ArgumentException">Throws when the regular expression match fails.</exception>
        public SemanticVersion UpdatePrereleaseTag(string prereleaseTag)
        {
            CheckValidPrereleaseOrBuildString(prereleaseTag, nameof(prereleaseTag));

            return new SemanticVersion(Major, Minor, Patch, prereleaseTag, null);
        }

        /// <summary>
        /// Set the build metadata.
        /// </summary>
        /// <example>UpdateBuildMetadata("001")</example>
        /// <example>UpdateBuildMetadata("20130313144700")</example>
        /// <example>UpdateBuildMetadata("exp.sha.5114f85")</example>
        /// <exception cref="ArgumentNullException">Throws when the parameter is null or whitespace.</exception>
        /// <exception cref="ArgumentException">Throws when the regular expression match fails.</exception>
        public SemanticVersion UpdateBuildMetadata(string buildMetadata)
        {
            CheckValidPrereleaseOrBuildString(buildMetadata, nameof(buildMetadata));

            return new SemanticVersion(Major, Minor, Patch, PrereleaseTag, buildMetadata);
        }

        /// <summary>
        /// Parses a semantic version from a string.
        /// </summary>
        /// <exception cref="ArgumentNullException">Throws when parameter is null or empty.</exception>
        /// <exception cref="ArgumentException">Throws when the regular expression match fails.</exception>
        /// <exception cref="FormatException">Throws when the major, minor, or patch version is not a valid integer.</exception>
        public static SemanticVersion Parse(string version)
        {
            Contract.CheckNonEmpty(version, nameof(version));

            Match match = SemVerRegex.Match(version);
            if (!match.Success)
            {
                throw new ArgumentException($"The version number \"{version}\" is not a valid semantic version.", nameof(version));
            }

            return new SemanticVersion(
                int.Parse(match.Groups["major"].Value),
                int.Parse(match.Groups["minor"].Value),
                int.Parse(match.Groups["patch"].Value),
                match.Groups["prerelease"].Success ? match.Groups["prerelease"].Value : null,
                match.Groups["build"].Success ? match.Groups["build"].Value : null);
        }

        /// <summary>
        /// Try to parse a semantic version from a string.
        /// </summary>
        /// <exception cref="ArgumentNullException">Throws when parameter is null or empty.</exception>
        /// <exception cref="ArgumentException">Throws when the regular expression match fails.</exception>
        /// <exception cref="FormatException">Throws when the major, minor, or patch version is not a number.</exception>
        public static bool TryParse(string version, out SemanticVersion result)
        {
            result = null;
            if (string.IsNullOrWhiteSpace(version))
            {
                return false;
            }

            Match match = SemVerRegex.Match(version);
            if (!match.Success)
            {
                return false;
            }

            int major, minor, patch;
            if (!int.TryParse(match.Groups["major"].Value, out major)
                || !int.TryParse(match.Groups["minor"].Value, out minor)
                || !int.TryParse(match.Groups["patch"].Value, out patch))
            {
                return false;
            }

            result = new SemanticVersion(
                major,
                minor,
                patch,
                match.Groups["prerelease"].Success ? match.Groups["prerelease"].Value : null,
                match.Groups["build"].Success ? match.Groups["build"].Value : null);

            return true;
        }

        internal static int ComparePrereleaseOrBuildVersionString(string left, string right)
        {
            bool existsLeft = !string.IsNullOrEmpty(left);
            bool existsRight = !string.IsNullOrEmpty(right);

            // When major, minor, and patch are equal, a pre-release/build version has lower precedence than a normal version
            if (existsLeft && !existsRight)
            {
                return -1;
            }
            if (!existsLeft && existsRight)
            {
                return 1;
            }
            if (!existsLeft && !existsRight)
            {
                return 0;
            }

            char[] dotDelimiter = new[] { '.' };
            string[] parts1 = left.Split(dotDelimiter, StringSplitOptions.RemoveEmptyEntries);
            string[] parts2 = right.Split(dotDelimiter, StringSplitOptions.RemoveEmptyEntries);

            int max = Math.Max(parts1.Length, parts2.Length);

            // Identifiers consisting of only digits are compared numerically and identifiers with letters or hyphens
            // are compared lexically in ASCII sort order.
            for (int i = 0; i < max; i++)
            {
                // A larger set of fields has a higher precedence than a smaller set,
                // if all of the preceding identifiers are equal
                if (i == parts1.Length)
                {
                    return -1;
                }
                if (i == parts2.Length)
                {
                    return 1;
                }

                string part1 = parts1[i];
                string part2 = parts2[i];
                int result = 0;
                int p1, p2;
                if (int.TryParse(part1, out p1) && int.TryParse(part2, out p2))
                {
                    result = p1.CompareTo(p2);
                }
                else
                {
                    result = string.Compare(part1, part2, StringComparison.Ordinal);
                }

                if (result != 0)
                {
                    return result;
                }
            }

            return 0;
        }

        /// <summary>
        /// Easier way to check if this is a prerelease semantic version.
        /// </summary>
        public bool IsPrereleaseVersion()
        {
            return !string.IsNullOrEmpty(PrereleaseTag);
        }

        //
        // Interface Implementations and Overrides
        //

        public override string ToString()
        {
            return $"{Major}.{Minor}.{Patch}{(string.IsNullOrEmpty(PrereleaseTag) ? "" : $"-{PrereleaseTag}")}{(string.IsNullOrEmpty(BuildMetadata) ? "" : $"+{BuildMetadata}")}";
        }

        public override bool Equals(object obj)
        {
            SemanticVersion other = obj as SemanticVersion;
            if (other == null)
            {
                return false;
            }
            return Equals(other);
        }

        public override int GetHashCode()
        {
            return Hashing.CombineHash(
                Major.GetHashCode(),
                Minor.GetHashCode(),
                Patch.GetHashCode(),
                PrereleaseTag.GetHashCode());
            //// Build metadata SHOULD be ignored when determining version precedence.
            ////BuildMetadata.GetHashCode());
        }

        public bool Equals(SemanticVersion other)
        {
            if (ReferenceEquals(other, null))
            {
                return false;
            }

            return Major == other.Major
                && Minor == other.Minor
                && Patch == other.Patch
                && PrereleaseTag == other.PrereleaseTag;
            //// Build metadata SHOULD be ignored when determining version precedence.
            ////&& BuildMetadata == other.BuildMetadata);
        }

        public int CompareTo(SemanticVersion other)
        {
            Contract.CheckValue(other, nameof(other));

            if (ReferenceEquals(this, other))
            {
                return 0;
            }

            int result = Major.CompareTo(other.Major);
            if (result != 0)
            {
                return result;
            }

            result = Minor.CompareTo(other.Minor);
            if (result != 0)
            {
                return result;
            }

            result = Patch.CompareTo(other.Patch);
            if (result != 0)
            {
                return result;
            }

            result = ComparePrereleaseOrBuildVersionString(PrereleaseTag, other.PrereleaseTag);
            if (result != 0)
            {
                return result;
            }

            // Build metadata SHOULD be ignored when determining version precedence
            return 0;
        }

        public static bool operator ==(SemanticVersion left, SemanticVersion right)
        {
            if (ReferenceEquals(left, null))
            {
                return ReferenceEquals(right, null);
            }
            if (ReferenceEquals(right, null))
            {
                return false;
            }

            return left.Equals(right);
        }

        public static bool operator !=(SemanticVersion left, SemanticVersion right)
        {
            return !(left == right);
        }

        public static bool operator <(SemanticVersion left, SemanticVersion right)
        {
            Contract.CheckValue(left, nameof(left));
            Contract.CheckValue(right, nameof(right));

            return left.CompareTo(right) < 0;
        }

        public static bool operator >=(SemanticVersion left, SemanticVersion right)
        {
            return !(left < right);
        }

        public static bool operator >(SemanticVersion left, SemanticVersion right)
        {
            Contract.CheckValue(left, nameof(left));
            Contract.CheckValue(right, nameof(right));

            return left.CompareTo(right) > 0;
        }

        public static bool operator <=(SemanticVersion left, SemanticVersion right)
        {
            return !(left > right);
        }
    }
}
