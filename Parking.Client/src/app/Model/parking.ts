export interface Parking {
    vehicleID: string,
    vehicle: string,
    parkingSpacesID:string,
    parkingSpaces: string,    
    entryDate: string,
    exitDate?: string,
    second?: number,
    duration?: string,
    totalValue?: string,
    vehicleTypeName: string,
    cubicCentimeters?:number,
    id: string    
}
