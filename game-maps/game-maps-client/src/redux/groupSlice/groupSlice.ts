import { createAsyncThunk, createSlice } from "@reduxjs/toolkit";
import { IGroups } from "../../models/group";
import axios from "../../services/axios";

const initialState: InitialState<IGroups[]> = {};

export const GetGroupsAndCategories = createAsyncThunk("GetGroupsAndCategories", async (slug: string) => {
    let res = await axios.get<IGroups[]>("/Group/GetGroupsWithCategoriesByGameSlug", { params: { slug } });
    return res.data;
})

const group = createSlice({
    name: "group",
    initialState,
    reducers: {},
    extraReducers: (b) => {
        b.addCase(GetGroupsAndCategories.pending, (state) => {
            state.loading = true;
        })
        b.addCase(GetGroupsAndCategories.fulfilled, (state, action) => {
            state.data = action.payload;
            state.loading = false;
        })
    }
})

export default group.reducer