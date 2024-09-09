import axios from 'axios';

export const ScheduleService = {
    async getSchedules() {
        let schedulesFromAxiosResult = await axios.get('https://localhost:6011/schedule')
        let schedulesDataFromResultToReturn = schedulesFromAxiosResult.data.schedules;
        return schedulesDataFromResultToReturn;
    }, async updateSchedule(scheduleForUpdate) {
        let updateScheduleAxiosResult = await axios.put('https://localhost:6011/schedule/' + scheduleForUpdate.id, 
            scheduleForUpdate)
        return updateScheduleAxiosResult.data;
    },
    async deleteSchedule(scheduleIdForDelete){
        let deleteScheduleAxiosResult = await axios.delete('https://localhost:6011/schedule/' + scheduleIdForDelete);
        return deleteScheduleAxiosResult.data;
    },
    async createSchedule(scheduleForAdd){
        let addScheduleAxiosResult = await axios.post('https://localhost:6011/schedule', scheduleForAdd);
        return addScheduleAxiosResult.data;
    }
}
