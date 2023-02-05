﻿using VacationRental.Application.Features.Rentals.AddRental.Domain;
using VacationRental.Application.Shared.Domain.Models;

namespace VacationRental.Application.Features.Rentals.AddRental.UseCase;

public interface IAddRentalUseCase
{
    Task<Rental> ExecuteAsync(AddRentalInput input, CancellationToken cancellationToken = default);
}
