export class Busy {
    public active: boolean = false;
    public text: string = '';
    public on(message) {
        this.text = message;
        this.active = true;
    }
    public off() {
        this.text = '';
        this.active = false;
    }
}
