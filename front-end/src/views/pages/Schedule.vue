<script setup>
import {FilterMatchMode} from '@primevue/core/api';
import {useToast} from 'primevue/usetoast';
import {onMounted, ref} from 'vue';
import {ScheduleService} from '@/service/ScheduleService';

onMounted(() => {
  ScheduleService.getSchedules().then(data => (schedule.value = data));
});

const toast = useToast();
const dt = ref();
const scheduleDialog = ref(false);
const deleteProductDialog = ref(false);
const createScheduleDialog = ref(false);
const scheduleForDoOperations = ref({});
const schedule = ref();

const filters = ref({
  global: {value: null, matchMode: FilterMatchMode.CONTAINS}
});
const submitted = ref(false);

function openNew() {
  scheduleForDoOperations.value = {};
  createScheduleDialog.value = true;
}

function hideDialog() {
  scheduleDialog.value = false;
  submitted.value = false;
}

async function updateSchedule() {
  const updateScheduleResult = await ScheduleService.updateSchedule(scheduleForDoOperations.value);
  submitted.value = true;
  scheduleDialog.value = false;
  scheduleForDoOperations.value = {};
  if (updateScheduleResult.isSuccess) {
    ScheduleService.getSchedules().then(data => (schedule.value = data));
    toast.add({severity: 'success', summary: 'Sucesso!', detail: 'Agendamento atualizado com sucesso!', life: 3000});
  }
}

function editSchedule(schedule) {
  scheduleForDoOperations.value = {...schedule};
  scheduleDialog.value = true;
}

function confirmDeleteSchedule(schedule) {
  scheduleForDoOperations.value = schedule;
  deleteProductDialog.value = true;
}

async function deleteSchedule() {
  const deleteScheduleResult = await ScheduleService.deleteSchedule(scheduleForDoOperations.value.id);
  deleteProductDialog.value = false;
  scheduleForDoOperations.value = {};
  if (deleteScheduleResult.isDeleted) {
    ScheduleService.getSchedules().then(data => (schedule.value = data));
    toast.add({severity: 'success', summary: 'Sucesso!', detail: 'Agendamento deletado com sucesso!', life: 3000});
  }
}

function hideDialogCreateDialog() {
  createScheduleDialog.value = false;
  submitted.value = false;
}

async function createSchedule() {
  const createScheduleResult = await ScheduleService.createSchedule(scheduleForDoOperations.value);
  submitted.value = true;
  createScheduleDialog.value = false;
  scheduleForDoOperations.value = {};
  if (createScheduleResult.id) {
    ScheduleService.getSchedules().then(data => (schedule.value = data));
    toast.add({severity: 'success', summary: 'Sucesso!', detail: 'Agendamento criado com sucesso!', life: 3000});
  }
}

</script>

<template>
  <div>
    <div class="card">
      <Toolbar class="mb-6">
        <template #start>
          <Button label="Adicionar" icon="pi pi-plus" severity="secondary" class="mr-2" @click="openNew"/>
        </template>

        <template #end>

        </template>
      </Toolbar>

      <DataTable
          ref="dt"
          :value="schedule"
          dataKey="id"
          :paginator="true"
          :rows="10"
          :filters="filters"
          paginatorTemplate="FirstPageLink PrevPageLink PageLinks NextPageLink LastPageLink CurrentPageReport RowsPerPageDropdown"
          :rowsPerPageOptions="[5, 10, 25]"
          currentPageReportTemplate="Mostrando {first} de {last} do {totalRecords} agendamentos"
      >
        <template #header>
          <div class="flex flex-wrap gap-2 items-center justify-between">
            <h4 class="m-0">Listar agendamentos</h4>
            <IconField>
              <InputIcon>
                <i class="pi pi-search"/>
              </InputIcon>
              <InputText v-model="filters['global'].value" placeholder="Pesquisar..."/>
            </IconField>
          </div>
        </template>

        <Column field="name" header="Nome" sortable style="min-width: 12rem"></Column>
        <Column field="phoneNumber" header="Telefone" sortable style="min-width: 16rem"></Column>
        <Column field="email" header="Email" sortable style="min-width: 16rem"></Column>
        <Column :exportable="false" style="min-width: 12rem">
          <template #body="slotProps">
            <Button icon="pi pi-pencil" outlined rounded class="mr-2" @click="editSchedule(slotProps.data)"/>
            <Button icon="pi pi-trash" outlined rounded severity="danger"
                    @click="confirmDeleteSchedule(slotProps.data)"/>
          </template>
        </Column>
      </DataTable>
    </div>

    <Dialog v-model:visible="scheduleDialog" :style="{ width: '450px' }" header="Detalhes do agendamento" :modal="true">
      <div class="flex flex-col gap-6">
        <div>
          <label for="name" class="block font-bold mb-3">Nome<small color="red">&nbsp; *</small></label>
          <InputText id="name" v-model.trim="scheduleForDoOperations.name" required="true" autofocus
                     :invalid="submitted && !scheduleForDoOperations.name" fluid/>
          <small v-if="submitted && !scheduleForDoOperations.name" class="text-red-500">Nome é obrigatório.</small>
        </div>
        <div>
          <label for="description" class="block font-bold mb-3">Telefone<small color="red">&nbsp;
            *</small></label>
          <InputText id="phoneNumber" v-model="scheduleForDoOperations.phoneNumber" required="true"
                     :invalid="submitted && !scheduleForDoOperations.phoneNumber" fluid/>
          <small v-if="submitted && !scheduleForDoOperations.phoneNumber" class="text-red-500">Telefone é
            obrigatório.</small>
        </div>
        <div>
          <label for="description" class="block font-bold mb-3">Email<small color="red">&nbsp; *</small></label>
          <InputText id="phoneNumber" type="email" v-model="scheduleForDoOperations.email" required="true"
                     :invalid="submitted && !scheduleForDoOperations.email" fluid/>
          <small v-if="submitted && !scheduleForDoOperations.email" class="text-red-500">Email é obrigatório.</small>
        </div>
      </div>

      <template #footer>
        <Button label="Fechar" icon="pi pi-times" text @click="hideDialog"/>
        <Button label="Salvar" icon="pi pi-check" @click="updateSchedule"/>
      </template>
    </Dialog>

    <Dialog v-model:visible="deleteProductDialog" :style="{ width: '450px' }" header="Confirmação" :modal="true">
      <div class="flex items-center gap-4">
        <i class="pi pi-exclamation-triangle !text-3xl"/>
        <span v-if="scheduleForDoOperations"
        >Você tem certeza que deseja remover o agendamento <b>{{ scheduleForDoOperations.name }}</b>?</span>
      </div>
      <template #footer>
        <Button label="Não" icon="pi pi-times" text @click="deleteProductDialog = false"/>
        <Button label="Sim" icon="pi pi-check" @click="deleteSchedule"/>
      </template>
    </Dialog>

    <Dialog v-model:visible="createScheduleDialog" :style="{ width: '450px' }" header="Criar do agendamento"
            :modal="true">
      <div class="flex flex-col gap-6">
        <div>
          <label for="name" class="block font-bold mb-3">Nome<small color="red">&nbsp; *</small></label>
          <InputText id="name" v-model.trim="scheduleForDoOperations.name" required="true" autofocus
                     :invalid="submitted && !scheduleForDoOperations.name" fluid/>
          <small v-if="submitted && !scheduleForDoOperations.name" class="text-red-500">Nome é obrigatório.</small>
        </div>
        <div>
          <label for="description" class="block font-bold mb-3">Telefone<small color="red">&nbsp;
            *</small></label>
          <InputText id="phoneNumber" v-model="scheduleForDoOperations.phoneNumber" required="true"
                     :invalid="submitted && !scheduleForDoOperations.phoneNumber" fluid/>
          <small v-if="submitted && !scheduleForDoOperations.phoneNumber" class="text-red-500">Telefone é
            obrigatório.</small>
        </div>
        <div>
          <label for="description" class="block font-bold mb-3">Email<small color="red">&nbsp; *</small></label>
          <InputText id="phoneNumber" type="email" v-model="scheduleForDoOperations.email" required="true"
                     :invalid="submitted && !scheduleForDoOperations.email" fluid/>
          <small v-if="submitted && !scheduleForDoOperations.email" class="text-red-500">Email é obrigatório.</small>
        </div>
      </div>

      <template #footer>
        <Button label="Fechar" icon="pi pi-times" text @click="hideDialogCreateDialog"/>
        <Button label="Salvar" icon="pi pi-check" @click="createSchedule"/>
      </template>
    </Dialog>
  </div>
</template>
