using Sirenix.Serialization;
using UnityEngine;

public class BlockDescriptor {
    [OdinSerialize]
    public bool HasPathOnTheLeft { get; set; }

    [OdinSerialize]
    public bool HasPathOnTheRight { get; set; }

    [OdinSerialize]
    public bool HasPathOnTheBottom { get; set; }

    [OdinSerialize]
    public bool HasPathOnTheTop { get; set; }

    public BlockDescriptor() {
    }

    public void ConnectPathPositions(Vector2Int currentPosition, Vector2Int otherPosition) {
        if (currentPosition.x > otherPosition.x) {
            this.HasPathOnTheLeft = true;
        } else if (currentPosition.x < otherPosition.x) {
            this.HasPathOnTheRight = true;
        } else if (currentPosition.y > otherPosition.y) {
            this.HasPathOnTheBottom = true;
        } else if (currentPosition.y < otherPosition.y) {
            this.HasPathOnTheTop = true;
        }
    }

    public bool RoomDescriptorSuffices(BlockDescriptor minReqs) {
        if (minReqs.HasPathOnTheBottom && !this.HasPathOnTheBottom) return false;
        if (minReqs.HasPathOnTheLeft && !this.HasPathOnTheLeft) return false;
        if (minReqs.HasPathOnTheRight && !this.HasPathOnTheRight) return false;
        if (minReqs.HasPathOnTheTop && !this.HasPathOnTheTop) return false;

        return true;
    }

    public override bool Equals(object obj) {
        return obj != null && Equals(obj as BlockDescriptor);
    }

    private bool Equals(BlockDescriptor other) {
        return HasPathOnTheLeft == other.HasPathOnTheLeft && HasPathOnTheRight == other.HasPathOnTheRight &&
               HasPathOnTheBottom == other.HasPathOnTheBottom && HasPathOnTheTop == other.HasPathOnTheTop;
    }

    public override int GetHashCode() {
        unchecked {
            var hashCode = HasPathOnTheLeft.GetHashCode();
            hashCode = (hashCode * 397) ^ HasPathOnTheRight.GetHashCode();
            hashCode = (hashCode * 397) ^ HasPathOnTheBottom.GetHashCode();
            hashCode = (hashCode * 397) ^ HasPathOnTheTop.GetHashCode();
            return hashCode;
        }
    }
}