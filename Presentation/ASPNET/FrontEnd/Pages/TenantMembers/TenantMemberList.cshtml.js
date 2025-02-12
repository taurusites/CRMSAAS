const App = {
    setup() {
        const state = Vue.reactive({
            mainData: [],
            deleteMode: false,
            tenantListLookupData: [],
            userListLookupData: [],
            mainTitle: null,
            id: '',
            name: '',
            tenantId: null,
            userId: null,
            description: '',
            errors: {
                name: '',
                tenantList: '',
                userList: ''
            },
            isSubmitting: false
        });

        const mainGridRef = Vue.ref(null);
        const mainModalRef = Vue.ref(null);
        const nameRef = Vue.ref(null);
        const tenantListRef = Vue.ref(null);
        const userListRef = Vue.ref(null);

        const nameText = {
            obj: null,
            create: () => {
                nameText.obj = new ej.inputs.TextBox({
                    placeholder: 'Enter Name',
                });
                nameText.obj.appendTo(nameRef.value);
            },
            refresh: () => {
                if (nameText.obj) {
                    nameText.obj.value = state.name;
                }
            }
        };

        const tenantListLookup = {
            obj: null,
            create: () => {
                if (state.tenantListLookupData && Array.isArray(state.tenantListLookupData)) {
                    tenantListLookup.obj = new ej.dropdowns.DropDownList({
                        dataSource: state.tenantListLookupData,
                        fields: { value: 'id', text: 'name' },
                        placeholder: 'Select a Tenant',
                        popupHeight: '200px',
                        filterBarPlaceholder: 'Search',
                        sortOrder: 'Ascending',
                        allowFiltering: true,
                        filtering: (e) => {
                            e.preventDefaultAction = true;
                            let query = new ej.data.Query();
                            if (e.text !== '') {
                                query = query.where('name', 'startsWith', e.text, true);
                            }
                            e.updateData(state.tenantListLookupData, query);
                        },
                        change: (e) => {
                            state.tenantId = e.value;
                        }
                    });
                    tenantListLookup.obj.appendTo(tenantListRef.value);
                } else {
                    console.error('Tenant list lookup data is not available or invalid.');
                }
            },
            refresh: () => {
                if (tenantListLookup.obj) {
                    tenantListLookup.obj.value = state.tenantId;
                }
            },
        };

        const userListLookup = {
            obj: null,
            create: () => {
                if (state.userListLookupData && Array.isArray(state.userListLookupData)) {
                    userListLookup.obj = new ej.dropdowns.DropDownList({
                        dataSource: state.userListLookupData,
                        fields: { value: 'id', text: 'email' },
                        placeholder: 'Select User',
                        popupHeight: '200px',
                        filterBarPlaceholder: 'Search',
                        sortOrder: 'Ascending',
                        allowFiltering: true,
                        filtering: (e) => {
                            e.preventDefaultAction = true;
                            let query = new ej.data.Query();
                            if (e.text !== '') {
                                query = query.where('email', 'startsWith', e.text, true);
                            }
                            e.updateData(state.userListLookupData, query);
                        },
                        change: (e) => {
                            state.userId = e.value;
                        }
                    });
                    userListLookup.obj.appendTo(userListRef.value);
                } else {
                    console.error('User list lookup data is not available or invalid.');
                }
            },
            refresh: () => {
                if (userListLookup.obj) {
                    userListLookup.obj.value = state.userId;
                }
            },
        };

        Vue.watch(
            () => state.name,
            (newVal, oldVal) => {
                state.errors.name = '';
                nameText.refresh();
            }
        );

        Vue.watch(
            () => state.tenantId,
            (newVal, oldVal) => {
                state.errors.tenantList = '';
                tenantListLookup.refresh();
            }
        );

        Vue.watch(
            () => state.userId,
            (newVal, oldVal) => {
                state.errors.userList = '';
                userListLookup.refresh();
            }
        );

        const validateForm = function () {
            state.errors.name = '';
            state.errors.tenantList = '';
            state.errors.userList = '';

            let isValid = true;

            if (!state.name) {
                state.errors.name = 'Name is required.';
                isValid = false;
            }
            if (!state.tenantId) {
                state.errors.tenantList = 'Tenant is required.';
                isValid = false;
            }
            if (!state.userId) {
                state.errors.userList = 'User is required.';
                isValid = false;
            }

            return isValid;
        };

        const resetFormState = () => {
            state.id = '';
            state.name = '';
            state.tenantId = null;
            state.userId = null;
            state.description = '';
            state.errors = {
                name: '',
                tenantList: '',
                userList: ''
            };
        };

        const services = {
            getMainData: async () => {
                try {
                    const response = await AxiosManager.get('/TenantMember/GetTenantMemberList', {});
                    return response;
                } catch (error) {
                    throw error;
                }
            },
            createMainData: async (userId, name, tenantId, description, createdById) => {
                try {
                    const response = await AxiosManager.post('/TenantMember/CreateTenantMember', {
                        userId, name, tenantId, description, createdById
                    });
                    return response;
                } catch (error) {
                    throw error;
                }
            },
            updateMainData: async (id, userId, name, tenantId, description, updatedById) => {
                try {
                    const response = await AxiosManager.post('/TenantMember/UpdateTenantMember', {
                        id, userId, name, tenantId, description, updatedById
                    });
                    return response;
                } catch (error) {
                    throw error;
                }
            },
            deleteMainData: async (id, deletedById) => {
                try {
                    const response = await AxiosManager.post('/TenantMember/DeleteTenantMember', {
                        id, deletedById
                    });
                    return response;
                } catch (error) {
                    throw error;
                }
            },
            getTenantListLookupData: async () => {
                try {
                    const response = await AxiosManager.get('/Tenant/GetTenantList', {});
                    return response;
                } catch (error) {
                    throw error;
                }
            },
            getUserListLookupData: async () => {
                try {
                    const response = await AxiosManager.get('/Security/GetSystemUserList', {});
                    return response;
                } catch (error) {
                    throw error;
                }
            },
        };

        const methods = {
            populateTenantListLookupData: async () => {
                const response = await services.getTenantListLookupData();
                state.tenantListLookupData = response?.data?.content?.data;
            },
            populateUserListLookupData: async () => {
                const response = await services.getUserListLookupData();
                state.userListLookupData = response?.data?.content?.data;
            },
            populateMainData: async () => {
                const response = await services.getMainData();
                state.mainData = response?.data?.content?.data.map(item => ({
                    ...item,
                    createdAtUtc: new Date(item.createdAtUtc)
                }));
            },
        };

        const handler = {
            handleSubmit: async function () {
                try {

                    state.isSubmitting = true;
                    await new Promise(resolve => setTimeout(resolve, 300));

                    if (!validateForm()) {
                        return;
                    }

                    const response = state.id === ''
                        ? await services.createMainData(state.userId, state.name, state.tenantId, state.description, StorageManager.getUserId())
                        : state.deleteMode
                            ? await services.deleteMainData(state.id, StorageManager.getUserId())
                            : await services.updateMainData(state.id, state.userId, state.name, state.tenantId, state.description, StorageManager.getUserId());

                    if (response.data.code === 200) {
                        await methods.populateMainData();
                        mainGrid.refresh();

                        if (!state.deleteMode) {
                            state.mainTitle = 'Edit Tenant Member';
                            state.id = response?.data?.content?.data.id ?? '';
                            state.name = response?.data?.content?.data.name ?? '';
                            state.tenantId = response?.data?.content?.data?.tenant?.id ?? '';
                            state.userId = response?.data?.content?.data.userId ?? '';
                            state.description = response?.data?.content?.data.description ?? '';

                            Swal.fire({
                                icon: 'success',
                                title: state.deleteMode ? 'Delete Successful' : 'Save Successful',
                                text: 'Form will be closed...',
                                timer: 2000,
                                showConfirmButton: false
                            });
                            setTimeout(() => {
                                mainModal.obj.hide();
                            }, 2000);

                        } else {
                            Swal.fire({
                                icon: 'success',
                                title: 'Delete Successful',
                                text: 'Form will be closed...',
                                timer: 2000,
                                showConfirmButton: false
                            });
                            setTimeout(() => {
                                mainModal.obj.hide();
                                resetFormState();
                            }, 2000);
                        }

                    } else {
                        Swal.fire({
                            icon: 'error',
                            title: state.deleteMode ? 'Delete Failed' : 'Save Failed',
                            text: response.data.message ?? 'Please check your data.',
                            confirmButtonText: 'Try Again'
                        });
                    }

                } catch (error) {
                    Swal.fire({
                        icon: 'error',
                        title: 'An Error Occurred',
                        text: error.response?.data?.message ?? 'Please try again.',
                        confirmButtonText: 'OK'
                    });
                } finally {
                    state.isSubmitting = false;
                }
            },
        };

        Vue.onMounted(async () => {
            try {
                await SecurityManager.authorizePage(['TenantMembers']);
                await SecurityManager.validateToken();

                await methods.populateMainData();
                await mainGrid.create(state.mainData);
                await methods.populateTenantListLookupData();
                tenantListLookup.create();
                await methods.populateUserListLookupData();
                userListLookup.create();
                nameText.create();

                mainModal.create();
                mainModalRef.value?.addEventListener('hidden.bs.modal', () => {
                    resetFormState();
                });

            } catch (e) {
                console.error('page init error:', e);
            } finally {
                hideSpinnerAndShowContent();
            }
        });

        Vue.onUnmounted(() => {
            mainModalRef.value?.removeEventListener('hidden.bs.modal', resetFormState);
        });

        const mainGrid = {
            obj: null,
            create: async (dataSource) => {
                mainGrid.obj = new ej.grids.Grid({
                    height: getDashminGridHeight(),
                    dataSource: dataSource,
                    allowFiltering: true,
                    allowSorting: true,
                    allowSelection: true,
                    allowGrouping: true,
                    groupSettings: {
                        columns: ['tenantName']
                    },
                    allowTextWrap: true,
                    allowResizing: true,
                    allowPaging: true,
                    allowExcelExport: true,
                    filterSettings: { type: 'CheckBox' },
                    sortSettings: { columns: [{ field: 'createdAtUtc', direction: 'Descending' }] },
                    pageSettings: { currentPage: 1, pageSize: 50, pageSizes: ["10", "20", "50", "100", "200", "All"] },
                    selectionSettings: { persistSelection: true, type: 'Single' },
                    autoFit: true,
                    showColumnMenu: true,
                    gridLines: 'Horizontal',
                    columns: [
                        { type: 'checkbox', width: 60 },
                        {
                            field: 'id', isPrimaryKey: true, headerText: 'Id', visible: false
                        },
                        { field: 'name', headerText: 'Name', width: 200, minWidth: 200 },
                        { field: 'userEmail', headerText: 'Email', width: 200, minWidth: 200 },
                        { field: 'tenantName', headerText: 'Tenant', width: 200, minWidth: 200 },
                        { field: 'description', headerText: 'Description', width: 400, minWidth: 400 },
                        { field: 'createdAtUtc', headerText: 'Created At UTC', width: 150, format: 'yyyy-MM-dd HH:mm' }
                    ],
                    toolbar: [
                        'ExcelExport', 'Search',
                        { type: 'Separator' },
                        { text: 'Add', tooltipText: 'Add', prefixIcon: 'e-add', id: 'AddCustom' },
                        { text: 'Edit', tooltipText: 'Edit', prefixIcon: 'e-edit', id: 'EditCustom' },
                        { text: 'Delete', tooltipText: 'Delete', prefixIcon: 'e-delete', id: 'DeleteCustom' },
                        { type: 'Separator' },
                    ],
                    beforeDataBound: () => { },
                    dataBound: function () {
                        mainGrid.obj.toolbarModule.enableItems(['EditCustom', 'DeleteCustom'], false);
                        mainGrid.obj.autoFitColumns(['name', 'userEmail', 'tenantName', 'description', 'createdAtUtc']);
                    },
                    excelExportComplete: () => { },
                    rowSelected: () => {
                        if (mainGrid.obj.getSelectedRecords().length == 1) {
                            mainGrid.obj.toolbarModule.enableItems(['EditCustom', 'DeleteCustom'], true);
                        } else {
                            mainGrid.obj.toolbarModule.enableItems(['EditCustom', 'DeleteCustom'], false);
                        }
                    },
                    rowDeselected: () => {
                        if (mainGrid.obj.getSelectedRecords().length == 1) {
                            mainGrid.obj.toolbarModule.enableItems(['EditCustom', 'DeleteCustom'], true);
                        } else {
                            mainGrid.obj.toolbarModule.enableItems(['EditCustom', 'DeleteCustom'], false);
                        }
                    },
                    rowSelecting: () => {
                        if (mainGrid.obj.getSelectedRecords().length) {
                            mainGrid.obj.clearSelection();
                        }
                    },
                    toolbarClick: async (args) => {
                        if (args.item.id === 'MainGrid_excelexport') {
                            mainGrid.obj.excelExport();
                        }

                        if (args.item.id === 'AddCustom') {
                            state.deleteMode = false;
                            state.mainTitle = 'Add Tenant Member';
                            resetFormState();
                            mainModal.obj.show();
                        }

                        if (args.item.id === 'EditCustom') {
                            state.deleteMode = false;
                            if (mainGrid.obj.getSelectedRecords().length) {
                                const selectedRecord = mainGrid.obj.getSelectedRecords()[0];
                                state.mainTitle = 'Edit Tenant Member';
                                state.id = selectedRecord.id ?? '';
                                state.name = selectedRecord.name ?? '';
                                state.tenantId = selectedRecord.tenant?.id ?? '';
                                state.userId = selectedRecord.userId ?? '';
                                state.description = selectedRecord.description ?? '';
                                mainModal.obj.show();
                            }
                        }

                        if (args.item.id === 'DeleteCustom') {
                            state.deleteMode = true;
                            if (mainGrid.obj.getSelectedRecords().length) {
                                const selectedRecord = mainGrid.obj.getSelectedRecords()[0];
                                state.mainTitle = 'Delete Tenant Member?';
                                state.id = selectedRecord.id ?? '';
                                state.name = selectedRecord.name ?? '';
                                state.tenantId = selectedRecord.tenant?.id ?? '';
                                state.userId = selectedRecord.userId ?? '';
                                state.description = selectedRecord.description ?? '';
                                mainModal.obj.show();
                            }
                        }
                    }
                });

                mainGrid.obj.appendTo(mainGridRef.value);
            },
            refresh: () => {
                mainGrid.obj.setProperties({ dataSource: state.mainData });
            }
        };

        const mainModal = {
            obj: null,
            create: () => {
                mainModal.obj = new bootstrap.Modal(mainModalRef.value, {
                    backdrop: 'static',
                    keyboard: false
                });
            }
        };

        return {
            mainGridRef,
            mainModalRef,
            nameRef,
            tenantListRef,
            userListRef,
            state,
            handler,
        };
    }
};

Vue.createApp(App).mount('#app');