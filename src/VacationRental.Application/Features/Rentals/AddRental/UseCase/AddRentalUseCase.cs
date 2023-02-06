using FluentValidation;
using Microsoft.Extensions.Logging;
using VacationRental.Application.Features.Rentals.AddRental.Domain;
using VacationRental.Application.Features.Rentals.AddRental.UseCase.Mapper;
using VacationRental.Application.Shared.Domain.Exceptions;
using VacationRental.Application.Shared.Domain.Models;
using VacationRental.Application.Shared.Extensions;
using VacationRental.Application.Shared.Repositories.Rentals;

namespace VacationRental.Application.Features.Rentals.AddRental.UseCase;

internal class AddRentalUseCase : IAddRentalUseCase
{
    private readonly ILogger<AddRentalUseCase> _logger;
    private readonly IValidator<AddRentalInput> _validator;
    private readonly IRentalRepository _rentalRepository;

    public AddRentalUseCase(ILogger<AddRentalUseCase> logger, IValidator<AddRentalInput> validator,
        IRentalRepository rentalRepository)
    {
        _logger = logger;
        _validator = validator;
        _rentalRepository = rentalRepository;
    }

    public async Task<Rental> ExecuteAsync(AddRentalInput input, CancellationToken cancellationToken = default)
    {
        _logger.LogDebug("Starting {useCase}", nameof(AddRentalUseCase));

        await EnsureValidInput(input, cancellationToken);

        var rental = input.ToRental();

        _logger.LogInformation("Adding new Rental: {@rental}", rental);
        await _rentalRepository.AddAsync(rental, cancellationToken);

        _logger.LogDebug("Finishing {useCase}", nameof(AddRentalUseCase));
        return rental;
    }

    private async Task EnsureValidInput(AddRentalInput input, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(input, nameof(input));

        var validationResult = await _validator.ValidateAsync(input, cancellationToken);
        if (!validationResult.IsValid)
        {
            _logger.Log(validationResult.Errors);
            throw new DataContractValidationException(validationResult);
        }
    }
}
