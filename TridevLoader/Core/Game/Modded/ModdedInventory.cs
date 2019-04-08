using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using RoR2;
using RoR2.Networking;
using TridevLoader.Mod.API;
using TridevLoader.Mod.API.Item;
using UnityEngine.Networking;

namespace TridevLoader.Core.Game.Modded {
    public class ModdedInventory : Inventory {
        //public readonly List<int> itemCounts;
        //public readonly Dictionary<ModObjectId, int> moddedItemAcquisitionOrder = new Dictionary<ModObjectId, int>();
//
        //public ModdedInventory() {
        //    this.itemCounts = new List<int>(ModItemRegistry.Instance.GetItemIds().Count);
        //}
//
        //private void Start() {
        //    if (!NetworkServer.active || !Run.instance.enabledArtifacts.HasArtifact(ArtifactIndex.Enigma))
        //        return;
        //    this.SetEquipmentIndex(EquipmentIndex.Enigma);
        //}
//
        //[Server]
        //public void GiveModdedItemByName(string itemString) {
        //    var modObjectId = new ModObjectId(itemString);
        //    if (!NetworkServer.active)
        //        return;
        //    if (modObjectId.ModId.Equals(VanillaMod.Id)) {
        //        base.GiveItemString(itemString);
        //    } else {
        //        this.GiveModdedItem(modObjectId);
        //    }
        //}
//
        //[Server]
        //public void GiveModdedItem(ModObjectId id, int count = 1) {
        //    if (!NetworkServer.active)
        //        return;
//
        //    if (count <= 0) {
        //        if (count >= 0)
        //            return;
        //        this.RemoveModdedItem(id, -count);
        //    } else {
        //        this.SetDirtyBit(1U);
        //        if ((this.itemCounts[(int) itemIndex] += count) == count) {
        //            this.itemAcquisitionOrder.Add(itemIndex);
        //            this.SetDirtyBit(8U);
        //        }
//
        //        Action inventoryChanged = this.onInventoryChanged;
        //        if (inventoryChanged != null)
        //            inventoryChanged();
        //        Action<Inventory, ItemIndex, int> onServerItemGiven = Inventory.onServerItemGiven;
        //        if (onServerItemGiven != null)
        //            onServerItemGiven(this, itemIndex, count);
        //        this.CallRpcItemAdded(itemIndex);
        //    }
        //}
//
        //public static event Action<Inventory, ItemIndex, int> onServerItemGiven;
//
        //[ClientRpc]
        //private void RpcItemAdded(ItemIndex itemIndex) {
        //    this.StartCoroutine(this.HighlightNewItem(itemIndex));
        //}
//
        //[Server]
        //public void RemoveModdedItem(ModObjectId id, int count = 0) {
        //    if (!NetworkServer.active)
        //        Debug.LogWarning((object) "[Server] function 'System.Void RoR2.Inventory::RemoveItem(RoR2.ItemIndex,System.Int32)' called on client");
        //    else if (count <= 0) {
        //        if (count >= 0)
        //            return;
        //        this.GiveModdedItem(id, -count);
        //    } else {
        //        int itemStack = this.itemCounts[(int) itemIndex];
        //        count = Math.Min(count, itemStack);
        //        if (count == 0)
        //            return;
        //        if ((this.itemCounts[(int) itemIndex] = itemStack - count) == 0) {
        //            this.itemAcquisitionOrder.Remove(itemIndex);
        //            this.SetDirtyBit(8U);
        //        }
//
        //        this.SetDirtyBit(1U);
        //        Action inventoryChanged = this.onInventoryChanged;
        //        if (inventoryChanged == null)
        //            return;
        //        inventoryChanged();
        //    }
        //}
//
        //[Server]
        //public void ResetItem(ItemIndex itemIndex) {
        //    if (!NetworkServer.active) {
        //        Debug.LogWarning((object) "[Server] function 'System.Void RoR2.Inventory::ResetItem(RoR2.ItemIndex)' called on client");
        //    } else {
        //        if (this.itemCounts[(int) itemIndex] <= 0)
        //            return;
        //        this.itemCounts[(int) itemIndex] = 0;
        //        this.SetDirtyBit(1U);
        //        this.SetDirtyBit(8U);
        //        Action inventoryChanged = this.onInventoryChanged;
        //        if (inventoryChanged == null)
        //            return;
        //        inventoryChanged();
        //    }
        //}
//
        //[Server]
        //public void CopyEquipmentFrom(Inventory other) {
        //    if (!NetworkServer.active) {
        //        Debug.LogWarning((object) "[Server] function 'System.Void RoR2.Inventory::CopyEquipmentFrom(RoR2.Inventory)' called on client");
        //    } else {
        //        for (int index = 0; index < other.equipmentStateSlots.Length; ++index)
        //            this.SetEquipment(new EquipmentState(other.equipmentStateSlots[index].equipmentIndex, Run.FixedTimeStamp.negativeInfinity, 1), (uint) index);
        //    }
        //}
//
        //[Server]
        //public void CopyItemsFrom(Inventory other) {
        //    if (!NetworkServer.active) {
        //        Debug.LogWarning((object) "[Server] function 'System.Void RoR2.Inventory::CopyItemsFrom(RoR2.Inventory)' called on client");
        //    } else {
        //        other.itemStacks.CopyTo((Array) this.itemCounts, 0);
        //        this.itemAcquisitionOrder.Clear();
        //        this.itemAcquisitionOrder.AddRange(other.itemAcquisitionOrder);
        //        this.SetDirtyBit(1U);
        //        this.SetDirtyBit(8U);
        //        Action inventoryChanged = this.onInventoryChanged;
        //        if (inventoryChanged == null)
        //            return;
        //        inventoryChanged();
        //    }
        //}
//
        //[Server]
        //public void ShrineRestackInventory([NotNull] Xoroshiro128Plus rng) {
        //    if (!NetworkServer.active) {
        //        Debug.LogWarning((object) "[Server] function 'System.Void RoR2.Inventory::ShrineRestackInventory(Xoroshiro128Plus)' called on client");
        //    } else {
        //        List<ItemIndex> itemIndexList1 = new List<ItemIndex>();
        //        List<ItemIndex> itemIndexList2 = new List<ItemIndex>();
        //        List<ItemIndex> itemIndexList3 = new List<ItemIndex>();
        //        List<ItemIndex> itemIndexList4 = new List<ItemIndex>();
        //        List<ItemIndex> itemIndexList5 = new List<ItemIndex>();
        //        List<ItemIndex> itemIndexList6 = new List<ItemIndex>();
        //        int count1 = 0;
        //        int count2 = 0;
        //        int count3 = 0;
        //        int count4 = 0;
        //        int count5 = 0;
        //        for (int index = 0; index < this.itemCounts.Length; ++index) {
        //            ItemIndex itemIndex = (ItemIndex) index;
        //            if (this.itemCounts[index] > 0) {
        //                switch (ItemCatalog.GetItemDef(itemIndex).tier) {
        //                    case ItemTier.Tier1:
        //                        count1 += this.itemCounts[index];
        //                        itemIndexList1.Add(itemIndex);
        //                        break;
        //                    case ItemTier.Tier2:
        //                        count2 += this.itemCounts[index];
        //                        itemIndexList2.Add(itemIndex);
        //                        break;
        //                    case ItemTier.Tier3:
        //                        count3 += this.itemCounts[index];
        //                        itemIndexList3.Add(itemIndex);
        //                        break;
        //                    case ItemTier.Lunar:
        //                        count4 += this.itemCounts[index];
        //                        itemIndexList4.Add(itemIndex);
        //                        break;
        //                    case ItemTier.Boss:
        //                        count5 += this.itemCounts[index];
        //                        itemIndexList5.Add(itemIndex);
        //                        break;
        //                }
        //            }
//
        //            this.ResetItem(itemIndex);
        //        }
//
        //        ItemIndex itemIndex1 = itemIndexList1.Count == 0 ? ItemIndex.None : itemIndexList1[rng.RangeInt(0, itemIndexList1.Count)];
        //        ItemIndex itemIndex2 = itemIndexList2.Count == 0 ? ItemIndex.None : itemIndexList2[rng.RangeInt(0, itemIndexList2.Count)];
        //        ItemIndex itemIndex3 = itemIndexList3.Count == 0 ? ItemIndex.None : itemIndexList3[rng.RangeInt(0, itemIndexList3.Count)];
        //        ItemIndex itemIndex4 = itemIndexList4.Count == 0 ? ItemIndex.None : itemIndexList4[rng.RangeInt(0, itemIndexList4.Count)];
        //        ItemIndex itemIndex5 = itemIndexList5.Count == 0 ? ItemIndex.None : itemIndexList5[rng.RangeInt(0, itemIndexList5.Count)];
        //        this.itemAcquisitionOrder.Clear();
        //        this.SetDirtyBit(8U);
        //        this.GiveItem(itemIndex1, count1);
        //        this.GiveItem(itemIndex2, count2);
        //        this.GiveItem(itemIndex3, count3);
        //        this.GiveItem(itemIndex4, count4);
        //        this.GiveItem(itemIndex5, count5);
        //    }
        //}
//
        //public int GetItemCount(ItemIndex itemIndex) {
        //    return this.itemCounts[(int) itemIndex];
        //}
//
        //public bool HasAtLeastXTotalItemsOfTier(ItemTier itemTier, int x) {
        //    int num = 0;
        //    for (ItemIndex itemIndex = ItemIndex.Syringe; itemIndex < ItemIndex.Count; ++itemIndex) {
        //        if (ItemCatalog.GetItemDef(itemIndex).tier == itemTier) {
        //            num += this.GetItemCount(itemIndex);
        //            if (num >= x)
        //                return true;
        //        }
        //    }
//
        //    return false;
        //}
//
        //public int GetTotalItemCountOfTier(ItemTier itemTier) {
        //    int num = 0;
        //    for (ItemIndex itemIndex = ItemIndex.Syringe; itemIndex < ItemIndex.Count; ++itemIndex) {
        //        if (ItemCatalog.GetItemDef(itemIndex).tier == itemTier)
        //            num += this.GetItemCount(itemIndex);
        //    }
//
        //    return num;
        //}
//
        //public void WriteItemStacks(int[] output) {
        //    Array.Copy((Array) this.itemCounts, output, output.Length);
        //}
//
        //public override int GetNetworkChannel() {
        //    return QosChannelIndex.defaultReliable.intVal;
        //}
//
        //public override void OnDeserialize(NetworkReader reader, bool initialState) {
        //    int num1 = reader.ReadByte();
        //    bool flag1 = (uint) (num1 & 1) > 0U;
        //    bool flag2 = (uint) (num1 & 4) > 0U;
        //    bool flag3 = (uint) (num1 & 8) > 0U;
        //    bool flag4 = (uint) (num1 & 16) > 0U;
        //    if (flag1)
        //        reader.ReadItemStacks(this.itemCounts);
        //    if (flag2)
        //        this.infusionBonus = reader.ReadPackedUInt32();
        //    if (flag3) {
        //        byte num2 = reader.ReadByte();
        //        this.itemAcquisitionOrder.Clear();
        //        this.itemAcquisitionOrder.Capacity = num2;
        //        for (byte index = 0; (int) index < (int) num2; ++index)
        //            this.itemAcquisitionOrder.Add((ItemIndex) reader.ReadByte());
        //    }
//
        //    if (flag4) {
        //        uint num2 = reader.ReadByte();
        //        for (uint slot = 0; slot < num2; ++slot)
        //            this.SetEquipmentInternal(EquipmentState.Deserialize(reader), slot);
        //        this.activeEquipmentSlot = reader.ReadByte();
        //    }
//
        //    if (!(flag1 | flag4 | flag2))
        //        return;
        //    Action inventoryChanged = this.onInventoryChanged;
        //    if (inventoryChanged == null)
        //        return;
        //    inventoryChanged();
        //}
//
        //public override bool OnSerialize(NetworkWriter writer, bool initialState) {
        //    uint num1 = this.syncVarDirtyBits;
        //    if (initialState)
        //        num1 = 17U;
        //    for (int index = 0; index < this.equipmentStateSlots.Length; ++index) {
        //        if (this.equipmentStateSlots[index].dirty) {
        //            num1 |= 16U;
        //            break;
        //        }
        //    }
//
        //    int num2 = (num1 & 1U) > 0U ? 1 : 0;
        //    bool flag1 = (num1 & 4U) > 0U;
        //    bool flag2 = (num1 & 8U) > 0U;
        //    bool flag3 = (num1 & 16U) > 0U;
        //    writer.Write((byte) num1);
        //    if (num2 != 0)
        //        writer.WriteItemStacks(this.itemCounts);
        //    if (flag1)
        //        writer.WritePackedUInt32(this.infusionBonus);
        //    if (flag2) {
        //        byte count = (byte) this.itemAcquisitionOrder.Count;
        //        writer.Write(count);
        //        for (byte index = 0; (int) index < (int) count; ++index)
        //            writer.Write((byte) this.itemAcquisitionOrder[index]);
        //    }
//
        //    if (flag3) {
        //        writer.Write((byte) this.equipmentStateSlots.Length);
        //        for (int index = 0; index < this.equipmentStateSlots.Length; ++index)
        //            EquipmentState.Serialize(writer, this.equipmentStateSlots[index]);
        //        writer.Write(this.activeEquipmentSlot);
        //    }
//
        //    if (!initialState) {
        //        for (int index = 0; index < this.equipmentStateSlots.Length; ++index)
        //            this.equipmentStateSlots[index].dirty = false;
        //    }
//
        //    if (!initialState)
        //        return num1 > 0U;
        //    return false;
        //}
//
        //protected static void InvokeRpcRpcItemAdded(NetworkBehaviour obj, NetworkReader reader) {
        //    if (!NetworkClient.active)
        //        Debug.LogError((object) "RPC RpcItemAdded called on server.");
        //    else
        //        ((Inventory) obj).RpcItemAdded((ItemIndex) reader.ReadInt32());
        //}
//
        //public void CallRpcItemAdded(ItemIndex itemIndex) {
        //    if (!NetworkServer.active) {
        //        Debug.LogError((object) "RPC Function RpcItemAdded called on client.");
        //    } else {
        //        NetworkWriter writer = new NetworkWriter();
        //        writer.Write((short) 0);
        //        writer.Write((short) 2);
        //        writer.WritePackedUInt32((uint) Inventory.kRpcRpcItemAdded);
        //        writer.Write(this.GetComponent<NetworkIdentity>().netId);
        //        writer.Write((int) itemIndex);
        //        this.SendRPCInternal(writer, 0, "RpcItemAdded");
        //    }
        //}
//
        //static ModdedInventory() {
        //    RegisterRpcDelegate(typeof(Inventory), Inventory.kRpcRpcItemAdded, new CmdDelegate(Inventory.InvokeRpcRpcItemAdded));
        //    NetworkCRC.RegisterBehaviour(nameof(Inventory), 0);
        //}
    }
}